import { UserService } from '../../scripts/services/UserService.js';

// DOM elements
const loginForm = document.getElementById("loginForm");
const registerForm = document.getElementById("registerForm");
const loginToggleBtn = document.getElementById("loginToggleBtn");
const registerToggleBtn = document.getElementById("registerToggleBtn");

// Toggle logic
function toggleForms(showLogin) {
    loginForm.classList.toggle("hidden", !showLogin);
    registerForm.classList.toggle("hidden", showLogin);
}

// Attach toggle event listeners
loginToggleBtn.addEventListener("click", () => toggleForms(true));
registerToggleBtn.addEventListener("click", () => toggleForms(false));

// Initialize service
const userService = new UserService();

// Login form submission
loginForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;

    const result = await userService.login(email, password);
    if (result.success) {
        window.location.href = "../ProductPage/Product.html";
    } else {
        alert("Login failed");
    }
});

// Register form submission
registerForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    const firstName = document.getElementById("firstName").value;
    const lastName = document.getElementById("lastName").value;
    const email = document.getElementById("registerEmail").value;
    const password = document.getElementById("registerPassword").value;

    const result = await userService.Register({ firstName, lastName, email, password });
    if (result.success) {
        window.location.href = "../ProductPage/Product.html";
    } else {
        alert("Registration failed");
    }
});
