import { CurrencyPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductServices } from '../../Services/product-services';
import { Producto } from '../../Models/productos';

@Component({
  imports: [CurrencyPipe, FormsModule],
  selector: 'app-home',
  styleUrl: './home.css',
  templateUrl: './home.html',
})
export class Home {
  private productServices = inject(ProductServices);

  public products: Producto[] = [];
  public editingId: number | null = null;
  public formModel: Producto = this.createEmptyProduct();
  public isLoading = false;
  public errorMessage = '';
  public successMessage = '';

  ngOnInit(): void {
    this.loadProducts();
  }

  private createEmptyProduct(): Producto {
    return {
      id: 0,
      nombre: '',
      cantidad: 1,
      valor: 0,
    };
  }

  private loadProducts(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.productServices.getProducts().subscribe({
      next: (data) => {
        this.products = data;
        this.isLoading = false;
        console.log('Productos cargados:', data);
      },
      error: (error) => {
        console.error('Error al cargar productos:', error);
        this.products = [];
        this.isLoading = false;
        this.errorMessage = 'No se pudo cargar el catálogo de café. Verifica que la API esté en ejecución.';
      },
    });
  }

  submitProduct(): void {
    const nombre = this.formModel.nombre.trim();
    const cantidad = Number(this.formModel.cantidad);
    const valor = Number(this.formModel.valor);

    if (!nombre) {
      this.errorMessage = 'El nombre del producto es obligatorio.';
      return;
    }

    if (!Number.isFinite(cantidad) || cantidad < 0) {
      this.errorMessage = 'La cantidad debe ser un número mayor o igual a 0.';
      return;
    }

    if (!Number.isFinite(valor) || valor <= 0) {
      this.errorMessage = 'El precio debe ser mayor que 0.';
      return;
    }

    const payload: Producto = {
      id: this.editingId ?? 0,
      nombre,
      cantidad,
      valor,
    };

    this.errorMessage = '';
    this.successMessage = '';

    if (this.editingId !== null) {
      this.isLoading = true;
      this.productServices.updateProduct(this.editingId, payload).subscribe({
        next: () => {
          this.successMessage = 'Producto actualizado correctamente.';
          this.resetForm();
          this.loadProducts();
        },
        error: (error) => {
          console.error('Error al actualizar producto:', error);
          this.errorMessage = 'No se pudo actualizar el producto. Revisa la conexión con la API.';
          this.isLoading = false;
        },
      });
      return;
    }

    this.isLoading = true;
    this.productServices.createProduct(payload).subscribe({
      next: () => {
        this.successMessage = 'Producto guardado correctamente.';
        this.resetForm();
        this.loadProducts();
      },
      error: (error) => {
        console.error('Error al crear producto:', error);
        this.errorMessage = 'No se pudo guardar el producto. Revisa la conexión con la API.';
        this.isLoading = false;
      },
    });
  }

  editProduct(product: Producto): void {
    this.editingId = product.id;
    this.formModel = { ...product };
    this.errorMessage = '';
    this.successMessage = '';
  }

  deleteProduct(id: number): void {
    if (!confirm('¿Deseas eliminar este producto del catálogo?')) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.productServices.deleteProduct(id).subscribe({
      next: () => {
        this.successMessage = 'Producto eliminado correctamente.';
        this.loadProducts();
        if (this.editingId === id) {
          this.resetForm();
        }
      },
      error: (error) => {
        console.error('Error al eliminar producto:', error);
        this.errorMessage = 'No se pudo eliminar el producto. Intenta nuevamente.';
        this.isLoading = false;
      },
    });
  }

  resetForm(): void {
    this.formModel = this.createEmptyProduct();
    this.editingId = null;
    this.isLoading = false;
  }
}
