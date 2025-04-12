import { UserService } from "./UserService.js";
import { apiBase } from "../env.js";

const userService = new UserService();


export class ProfileService {
    refreshToken = localStorage.getItem('refreshToken');
    async LogOut (){
        userService.authorizedFetch(`${apiBase}/Profile/LogOut`,{
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(this.refreshToken),
        });
        localStorage.setItem('refreshToken', null);
    }
}