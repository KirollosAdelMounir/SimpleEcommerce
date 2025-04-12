import { apiBase } from "../env.js";
import { UserService  } from "./UserService.js";

const userService = new UserService();


export class OrderService {

    async getAllOrders(userId = "", status = 1) {
        const query = userId ? `?UserId=${userId}&status=${status}` : `?status=${status}`;
        const res = await userService.authorizedFetch(`${apiBase}/Order/GetAllOrders${query}`);
        return await res.json();
    }

    async getOrderDetails(orderId) {
        const res = await userService.authorizedFetch(`${apiBase}/Order/GetOrderDetails?Id=${orderId}`);
        return await res.json();
    }

    async addToCart(productId, requestQty) {
        const payload = {
            productId,
            requestQty
        };
        const res = await userService.authorizedFetch(`${apiBase}/Order/AddToCart`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });
        return res;
    }

    async changeOrderStatus(orderId, status) {
        const res = await userService.authorizedFetch(`${apiBase}/Order/ChangeOrderStatus?Id=${orderId}&status=${status}`, {
            method: 'PUT'
        });
        return await res;
    }
}
