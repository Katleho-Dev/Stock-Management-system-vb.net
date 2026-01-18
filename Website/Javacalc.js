// Wait until the page loads
document.addEventListener("DOMContentLoaded", function () {
    const orderForm = document.getElementById("orderForm");

    // When form is submitted
    orderForm.addEventListener("submit", function (e) {
        e.preventDefault(); // stop page refresh

        // Get values
        const name = document.getElementById("customerName").value;
        const productSelect = document.getElementById("product");
        const productText = productSelect.options[productSelect.selectedIndex].text;
        const productPrice = parseFloat(productSelect.value);
        const quantity = parseInt(document.getElementById("quantity").value);

        // Get payment option
        let paymentMethod = "";
        const paymentOptions = document.querySelectorAll("input[name='payment']");
        paymentOptions.forEach(option => {
            if (option.checked) {
                paymentMethod = option.value;
            }
        });

        // Terms checkbox
        const termsAccepted = document.getElementById("terms").checked;

        // Validation (basic)
        if (name === "" || !paymentMethod || !termsAccepted) {
            alert("⚠️ Please fill in your name, select payment, and accept terms.");
            return;
        }

        // Calculate total
        const subtotal = productPrice * quantity;

        // Show results in alert (for now)
        alert(
            `✅ Order Summary:\n` +
            `Name: ${name}\n` +
            `Product: ${productText}\n` +
            `Quantity: ${quantity}\n` +
            `Payment: ${paymentMethod}\n` +
            `Total: R${subtotal.toFixed(2)}`
        );
    });
});

// Store cart items
let cart = [];
const VAT_RATE = 0.15; // 15% VAT

document.addEventListener("DOMContentLoaded", function () {
    const orderForm = document.getElementById("orderForm");

    // Add to cart when form is submitted
    orderForm.addEventListener("submit", function (e) {
        e.preventDefault(); // stop page refresh

        // Get values
        const name = document.getElementById("customerName").value;
        const productSelect = document.getElementById("product");
        const productText = productSelect.options[productSelect.selectedIndex].text;
        const productPrice = parseFloat(productSelect.value);
        const quantity = parseInt(document.getElementById("quantity").value);

        let paymentMethod = "";
        const paymentOptions = document.querySelectorAll("input[name='payment']");
        paymentOptions.forEach(option => {
            if (option.checked) paymentMethod = option.value;
        });

        const termsAccepted = document.getElementById("terms").checked;

        // Validation
        if (name === "" || !paymentMethod || !termsAccepted) {
            alert("⚠️ Please fill in your name, select payment, and accept terms.");
            return;
        }

        // Add product to cart
        cart.push({ name: productText, price: productPrice, qty: quantity });

        // Update cart display
        displayCart();
    });
});

// Display cart on the page
function displayCart() {
    const cartList = document.getElementById("cart");
    cartList.innerHTML = ""; // clear previous

    let subtotal = 0;

    cart.forEach((item, index) => {
        subtotal += item.price * item.qty;

        let li = document.createElement("li");
        li.textContent = `${item.qty} x ${item.name} = R${(item.price * item.qty).toFixed(2)}`;

        // Add remove button
        let btn = document.createElement("button");
        btn.textContent = "X";
        btn.style.marginLeft = "10px";
        btn.onclick = () => {
            cart.splice(index, 1); // remove item
            displayCart();
        };

        li.appendChild(btn);
        cartList.appendChild(li);
    });

    // Calculate totals
    let vat = subtotal * VAT_RATE;
    let total = subtotal + vat;

    document.getElementById("totals").textContent =
        `Subtotal: R${subtotal.toFixed(2)} | VAT (15%): R${vat.toFixed(2)} | Total: R${total.toFixed(2)}`;
}

// Checkout
function checkout() {
    if (cart.length === 0) {
        alert("⚠️ Your cart is empty!");
        return;
    }

    alert("✅ Thank you for your order! Your items will be ready soon.");
    cart = []; // clear cart
    displayCart(); // refresh
}