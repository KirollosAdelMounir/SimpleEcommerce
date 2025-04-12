import { apiBase } from "../env.js";
import { UserService } from "./UserService.js";

const userService = new UserService();

export class ProductService {
    async loadProducts() {
        const res = await userService.authorizedFetch(`${apiBase}/Product/GetProducts`);
        return await res.json();       
      }
    async ViewProduct(id){
        const res = await userService.authorizedFetch(`${apiBase}/Product/GetProducts?Id=${id}`);
        return await res.json();      
    }
    async AddProduct(name , description , price , availableQuantity){
        const res = await userService.authorizedFetch(`${apiBase}/Product/AddNewProduct`,
            {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body : JSON.stringify({name,description,price,availableQuantity})
            }
        );
        return res;
    }
}