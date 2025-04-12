import { apiBase } from '../env.js';

export class UserService {
  constructor() {
    this.apiBase = apiBase;
    this.accessToken = localStorage.getItem('accessToken');
    this.refreshToken = localStorage.getItem('refreshToken');
  }

  async login(email, password) {
    const res = await fetch(`${this.apiBase}/User/Login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password }),
    });

    const data = await res.json();

    if (res.ok) {
      this.accessToken = data.accessToken;
      this.refreshToken = data.refreshToken;

      localStorage.setItem('accessToken', this.accessToken);
      localStorage.setItem('refreshToken', this.refreshToken);
      return { success: true };
    } else {
      alert(typeof data === 'string' ? data : data?.message || 'Login failed.');
      return { success: false, message: data };
    }
  }

  async Register({ firstName, lastName, email, password }) {
    const res = await fetch(`${this.apiBase}/User/Register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ firstName, lastName, email, password }),
    });

    const data = await res.json();

    if (res.ok) {
      return await this.login(email, password);
    } else {
      alert(typeof data === 'string' ? data : data?.message || 'Registration failed.');
      return { success: false, message: data };
    }
  }

  async refreshAccessToken() {
    const res = await fetch(`${this.apiBase}/User/RefreshToken`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ refreshToken: this.refreshToken }),
    });

    const data = await res.json();

    if (res.ok) {
      this.token = data.token;
      localStorage.setItem('accessToken', this.accessToken);
      return true;
    } else {
      alert('Session expired. Please login again.');
      return false;
    }
  }

  async authorizedFetch(url, options = {}) {
    options.headers = options.headers || {};
    options.headers['Authorization'] = `Bearer ${this.accessToken}`;

    let res = await fetch(url, options);

    if (res.status === 401 && this.refreshToken) {
      const refreshed = await this.refreshAccessToken();
      if (refreshed) {
        options.headers['Authorization'] = `Bearer ${this.accessToken}`;
        res = await fetch(url, options);
      }
    }

    return res;
  }
  isAdmin(){
    const token = localStorage.getItem('accessToken');
    const claims = this.parseJwt(token);
    return claims && claims.role === 'Admin';
  }
  parseJwt(token) {
    try {
      const base64Url = token.split('.')[1]; // payload is the second part
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map(c => `%${('00' + c.charCodeAt(0).toString(16)).slice(-2)}`)
          .join('')
      );
      return JSON.parse(jsonPayload);
    } catch (err) {
      console.error("Invalid token", err);
      return null;
    }
  }

}
