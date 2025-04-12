import { ProductService } from '../../scripts/services/ProductService.js';
import { OrderService } from '../../scripts/services/OrderService.js';
import { UserService } from '../../scripts/services/UserService.js';

const productService = new ProductService();
const orderService = new OrderService();
const userService = new UserService();

function LoadPage() {
    loadProducts();
    checkIsAdmin();
}
async function checkIsAdmin() {
    const isAdmin = userService.isAdmin();
    const productList = $('#product-list');       // Fixed: Defined productList

    if (isAdmin) {
      const adminControls = `
        <div class="text-end mb-4" id="admin-controls">
          <button class="btn btn-success" id="toggle-add-form">Add Product</button>
          <div id="add-product-form" class="card p-3 mt-3 d-none shadow">
            <input type="text" id="product-name" class="form-control mb-2" placeholder="Product Name" />
            <textarea id="product-description" class="form-control mb-2" placeholder="Description"></textarea>
            <input type="number" id="product-price" class="form-control mb-2" placeholder="Price" />
            <input type="number" id="available-Quantity" class="form-control mb-2" placeholder="Quantity" />

            <button class="btn btn-primary" id="submit-product">Submit</button>
          </div>
        </div>
      `;
      productList.before(adminControls);

      $('#toggle-add-form').on('click', () => {
        $('#add-product-form').toggleClass('d-none');
      });

      $('#submit-product').on('click', async () => {
        const name = $('#product-name').val();
        const description = $('#product-description').val();
        const price = parseFloat($('#product-price').val());
        const quantity = parseInt($('#available-Quantity').val());


        if (!name || !description || isNaN(price)) {
          alert("Please fill all fields correctly.");
          return;
        }

        const res = await productService.AddProduct(name,description,price,quantity);
        if (res.ok) {
          alert("Product added successfully!");
          $('#add-product-form').addClass('d-none');
          loadProducts();
        } else {
          alert("Failed to add product.");
        }
      });
    }
}

async function loadProducts() {
  try {
    const products = await productService.loadProducts();
    const productList = $('#product-list');
    productList.empty();

    products.forEach(product => {
      const card = `
        <div class="col-md-4">
          <div class="card h-100 shadow-sm">
            <div class="card-body d-flex flex-column">
              <h5 class="card-title">${product.name}</h5>
              <p class="card-text">${product.description}</p>
              <p class="card-text fw-bold">$${product.price.toFixed(2)}</p>
              <button class="btn btn-primary mt-auto add-to-cart-btn" data-id="${product.id}">Add to Cart</button>
            </div>
          </div>
        </div>
      `;
      productList.append(card);
    });

    $('.add-to-cart-btn').on('click', function () {
      const id = $(this).data('id');
      addToCart(id);
    });

  } catch (err) {
    console.error('Failed to load products', err);
    alert('Could not load products.');
  }
}

async function addToCart(productId) {
  try {
    const res = await orderService.addToCart(productId, 1);
    if (res.ok) {
      alert("Added to cart!");
    } else {
      const error = await res.text();
      alert("Failed to add to cart: " + error);
    }
  } catch (err) {
    alert("Error adding to cart");
    console.error(err);
  }
}

window.LoadPage = LoadPage;
