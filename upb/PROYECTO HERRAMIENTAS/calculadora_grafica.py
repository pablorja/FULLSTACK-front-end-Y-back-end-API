import ast
import operator
import math
import tkinter as tk
from tkinter import ttk, messagebox

OPERATORS = {
    ast.Add: operator.add,
    ast.Sub: operator.sub,
    ast.Mult: operator.mul,
    ast.Div: operator.truediv,
    ast.Pow: operator.pow,
    ast.USub: operator.neg,
    ast.UAdd: operator.pos,
}

ALLOWED_NAMES = {
    'sqrt': math.sqrt,
    'sin': math.sin,
    'cos': math.cos,
    'tan': math.tan,
    'log': math.log10,
    'ln': math.log,
    'exp': math.exp,
    'abs': abs,
    'pi': math.pi,
    'e': math.e,
    'degrees': math.degrees,
    'radians': math.radians,
    'factorial': math.factorial,
    'floor': math.floor,
    'ceil': math.ceil,
}

class SafeEvaluator(ast.NodeVisitor):
    def visit(self, node):
        if isinstance(node, ast.Expression):
            return self.visit(node.body)
        return super().visit(node)

    def visit_BinOp(self, node):
        left = self.visit(node.left)
        right = self.visit(node.right)
        op_type = type(node.op)
        if op_type in OPERATORS:
            try:
                return OPERATORS[op_type](left, right)
            except ZeroDivisionError:
                raise ZeroDivisionError("División por cero")
        raise ValueError(f"Operador no permitido: {op_type}")

    def visit_UnaryOp(self, node):
        operand = self.visit(node.operand)
        op_type = type(node.op)
        if op_type in OPERATORS:
            return OPERATORS[op_type](operand)
        raise ValueError(f"Operador unario no permitido: {op_type}")

    def visit_Num(self, node):
        return node.n

    def visit_Constant(self, node):
        if isinstance(node.value, (int, float)):
            return node.value
        raise ValueError("Constante no numérica no permitida")

    def visit_Call(self, node):
        if not isinstance(node.func, ast.Name):
            raise ValueError("Llamada a función inválida")
        func_name = node.func.id
        if func_name not in ALLOWED_NAMES:
            raise ValueError(f"Función no permitida: {func_name}")
        func = ALLOWED_NAMES[func_name]
        if func_name in ('sin', 'cos', 'tan', 'log', 'ln', 'exp', 'degrees', 'radians'):
            if len(node.args) != 1:
                raise ValueError(f"{func_name} requiere 1 argumento")
            arg = self.visit(node.args[0])
            if func_name in ('sin', 'cos', 'tan', 'degrees', 'radians'):
                arg = math.radians(arg)
            return func(arg)
        elif func_name in ('sqrt', 'abs', 'floor', 'ceil', 'factorial'):
            if len(node.args) != 1:
                raise ValueError(f"{func_name} requiere 1 argumento")
            arg = self.visit(node.args[0])
            return func(arg)
        return func

    def visit_Name(self, node):
        if node.id in ALLOWED_NAMES:
            return ALLOWED_NAMES[node.id]
        raise ValueError(f"Nombre no permitido: {node.id}")

    def generic_visit(self, node):
        raise ValueError(f"Expresión no permitida: {node.__class__.__name__}")


def safe_eval(expr: str):
    parsed = ast.parse(expr, mode='eval')
    evaluator = SafeEvaluator()
    return evaluator.visit(parsed)


class ScientificCalculator:
    def __init__(self, root):
        self.root = root
        self.root.title("Calculadora Científica")
        self.root.geometry("400x600")
        self.root.resizable(False, False)
        
        self.expression = ""
        self.input_text = tk.StringVar()
        
        self.create_display()
        self.create_buttons()
        
    def create_display(self):
        input_frame = ttk.Frame(self.root, padding=10)
        input_frame.pack(fill=tk.BOTH, expand=True)
        
        self.input_field = ttk.Entry(
            input_frame,
            textvariable=self.input_text,
            font=("Arial", 18),
            justify="right"
        )
        self.input_field.pack(fill=tk.BOTH, expand=True)
        
    def create_buttons(self):
        button_frame = ttk.Frame(self.root, padding=10)
        button_frame.pack(fill=tk.BOTH, expand=True)
        
        buttons = [
            ("sin", 1, 0), ("cos", 1, 1), ("tan", 1, 2), ("log", 1, 3),
            ("ln", 2, 0), ("exp", 2, 1), ("sqrt", 2, 2), ("abs", 2, 3),
            ("(", 3, 0), (")", 3, 1), ("^", 3, 2), ("C", 3, 3),
            ("7", 4, 0), ("8", 4, 1), ("9", 4, 2), ("/", 4, 3),
            ("4", 5, 0), ("5", 5, 1), ("6", 5, 2), ("*", 5, 3),
            ("1", 6, 0), ("2", 6, 1), ("3", 6, 2), ("-", 6, 3),
            ("0", 7, 0), (".", 7, 1), ("=", 7, 2), ("+", 7, 3),
        ]
        
        for text, row, col in buttons:
            if text == "=":
                btn = ttk.Button(
                    button_frame,
                    text=text,
                    command=self.calculate
                )
            elif text == "C":
                btn = ttk.Button(
                    button_frame,
                    text=text,
                    command=self.clear
                )
            elif text in ("sin", "cos", "tan", "log", "ln", "exp", "sqrt", "abs"):
                btn = ttk.Button(
                    button_frame,
                    text=text,
                    command=lambda t=text: self.append_function(t)
                )
            else:
                btn = ttk.Button(
                    button_frame,
                    text=text,
                    command=lambda t=text: self.append_to_expression(t)
                )
            btn.grid(row=row, column=col, sticky="nsew", padx=3, pady=3)
        
        for i in range(8):
            button_frame.rowconfigure(i, weight=1)
        for i in range(4):
            button_frame.columnconfigure(i, weight=1)
    
    def append_to_expression(self, value):
        self.expression += str(value)
        self.input_text.set(self.expression)
    
    def append_function(self, func):
        self.expression += func + "("
        self.input_text.set(self.expression)
    
    def clear(self):
        self.expression = ""
        self.input_text.set("")
    
    def calculate(self):
        try:
            result = safe_eval(self.expression)
            if isinstance(result, float):
                if result == int(result):
                    result = int(result)
                else:
                    result = round(result, 10)
            self.input_text.set(str(result))
            self.expression = str(result)
        except ZeroDivisionError:
            messagebox.showerror("Error", "División por cero")
            self.expression = ""
            self.input_text.set("")
        except ValueError as e:
            messagebox.showerror("Error", str(e))
            self.expression = ""
            self.input_text.set("")
        except Exception as e:
            messagebox.showerror("Error", f"Expresión inválida")
            self.expression = ""
            self.input_text.set("")


def main():
    root = tk.Tk()
    app = ScientificCalculator(root)
    root.mainloop()


if __name__ == "__main__":
    main()