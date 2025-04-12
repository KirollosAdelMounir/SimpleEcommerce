import { OrderService } from "../../scripts/services/OrderService.js";
import { UserService } from "../../scripts/services/UserService.js";

const orderService = new OrderService();
const userService = new UserService();
const container = document.getElementById("ordersContainer");
const statusSelect = document.getElementById("orderStatus");

// Mapping of next status based on current status and role
const statusActions = {
  0: { 
    normalUser: { text: "Complete the Order", nextStatus: 1 },
    admin: { text: "Cancel Order", nextStatus: 5 } // Admin can cancel pending orders
  },
  1: { admin: { text: "Receive the Order", nextStatus: 2 } },
  2: { admin: { text: "Prepare the Order", nextStatus: 3 } },
  3: { admin: { text: "Deliver the Order", nextStatus: 4 } },
  4: { admin: { text: "Finish the Order", nextStatus: 1 } }
};

function getStatusText(status) {
  return ["Pending", "Completed", "Received", "Preparing", "Delivering", "Cancelled"][status] || "Unknown";
}

async function loadOrders() {
  container.innerHTML = "Loading orders...";
  const accessToken = localStorage.getItem('accessToken');
  const status = parseInt(statusSelect.value);
  const isAdmin = userService.isAdmin();
  const payload = userService.parseJwt(accessToken);

  const orders = isAdmin
    ? await orderService.getAllOrders(undefined, status)
    : await orderService.getAllOrders(payload.nameid, status);

  container.innerHTML = "";

  if (!orders.length) {
    container.innerHTML = "<p>No orders found.</p>";
    return;
  }

  orders.forEach(order => {
    const div = document.createElement("div");
    div.className = "order-card";

    const role = isAdmin ? "admin" : "normalUser";
    const actionConfig = statusActions[order.status]?.[role];
    const actionButtonHTML = actionConfig
      ? `<button class="btn btn-sm btn-success action-btn" data-id="${order.id}" data-next-status="${actionConfig.nextStatus}">${actionConfig.text}</button>`
      : "";

    const cancelButtonHTML = order.status === 0
      ? `<button class="btn btn-sm btn-danger cancel-btn" data-id="${order.id}">Cancel Order</button>`
      : "";

    div.innerHTML = `
      <h3>Order ID: ${order.id}</h3>
      <p>User: ${order.userName}</p>
      <p>Status: ${getStatusText(order.status)}</p>
      <button class="btn btn-sm btn-primary toggle-btn" data-id="${order.id}">View Details</button>
      ${actionButtonHTML}
      ${cancelButtonHTML}
      <div class="order-details" id="details-${order.id}"></div>
    `;
    container.appendChild(div);
  });

  document.querySelectorAll(".toggle-btn").forEach(btn => {
    btn.addEventListener("click", async () => {
      const orderId = btn.getAttribute("data-id");
      const detailsDiv = document.getElementById(`details-${orderId}`);

      if (detailsDiv.style.display === "block") {
        detailsDiv.style.display = "none";
        btn.textContent = "View Details";
      } else {
        const details = await orderService.getOrderDetails(orderId);
        detailsDiv.innerHTML = details.productItems.map(item => `
          <p><strong>Product:</strong> ${item.product.name}</p>
          <p><strong>Description:</strong> ${item.product.description}</p>
          <p><strong>Price:</strong> ${item.product.price}</p>
          <p><strong>Quantity:</strong> ${item.requestQty}</p>
          <hr />
        `).join('');
        detailsDiv.style.display = "block";
        btn.textContent = "Hide Details";
      }
    });
  });

  document.querySelectorAll(".action-btn").forEach(btn => {
    btn.addEventListener("click", async () => {
      const orderId = btn.getAttribute("data-id");
      const nextStatus = btn.getAttribute("data-next-status");

      if (confirm("Are you sure you want to change the order status?")) {
        const result = await orderService.changeOrderStatus(orderId, nextStatus);
        if (result.success) {
          alert("Order status updated.");
          loadOrders();
        } else {
          alert("Failed to update status.");
        }
      }
    });
  });

  // Handle cancel order button
  document.querySelectorAll(".cancel-btn").forEach(btn => {
    btn.addEventListener("click", async () => {
      const orderId = btn.getAttribute("data-id");

      if (confirm("Are you sure you want to cancel this order?")) {
        const result = await orderService.changeOrderStatus(orderId, 5); 
        if (result.ok) {
          alert("Order has been canceled.");
          loadOrders(); 
        } else {
          alert("Failed to cancel the order.");
        }
      }
    });
  });
}

statusSelect.addEventListener("change", loadOrders);
loadOrders();
