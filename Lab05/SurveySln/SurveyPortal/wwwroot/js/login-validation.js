document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    form.addEventListener("submit", function (e) {
        const errors = [];

        const email = form.querySelector("[name='Email']");
        const password = form.querySelector("[name='Password']");

        form.querySelectorAll(".text-danger").forEach(el => el.textContent = "");

        if (!email.value.trim()) {
            errors.push({ field: email, message: "Email is required." });
        } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value)) {
            errors.push({ field: email, message: "Invalid email format." });
        }

        if (!password.value) {
            errors.push({ field: password, message: "Password is required." });
        }

        if (errors.length > 0) {
            e.preventDefault();
            for (const err of errors) {
                const span = form.querySelector(`[data-valmsg-for='${err.field.name}']`) ||
                    form.querySelector(`[name='${err.field.name}'] + .text-danger`);
                if (span) span.textContent = err.message;
            }
        }
    });
});
