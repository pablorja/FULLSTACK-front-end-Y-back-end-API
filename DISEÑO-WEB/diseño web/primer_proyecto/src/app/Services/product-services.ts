import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Producto } from '../Models/productos';
import { map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ProductServices {
  private httpClient = inject(HttpClient);
  private apiUrl: string = environment.ApiUrl;

  getProducts() {
    return this.httpClient.get<ApiCafe[]>(`${this.apiUrl}cafes`).pipe(
      map((cafes) => cafes.map((cafe) => this.fromApiCafe(cafe))),
    );
  }

  createProduct(product: Producto) {
    return this.httpClient.post<ApiCafe>(`${this.apiUrl}cafes`, this.toApiCafe(product));
  }

  updateProduct(id: number, product: Producto) {
    return this.httpClient.put<void>(`${this.apiUrl}cafes/${id}`, this.toApiCafe(product));
  }

  deleteProduct(id: number) {
    return this.httpClient.delete<void>(`${this.apiUrl}cafes/${id}`);
  }

  private toApiCafe(product: Producto): ApiCafe {
    return {
      id: product.id,
      marcaId: 1,
      marca: '',
      nombre: product.nombre,
      origen: '',
      stock: product.cantidad,
      precio: product.valor,
    };
  }

  private fromApiCafe(cafe: ApiCafe): Producto {
    return {
      id: cafe.id,
      nombre: cafe.nombre,
      cantidad: cafe.stock,
      valor: cafe.precio,
    };
  }
}

interface ApiCafe {
  id: number;
  marcaId: number;
  marca: string;
  nombre: string;
  origen: string;
  stock: number;
  precio: number;
}
