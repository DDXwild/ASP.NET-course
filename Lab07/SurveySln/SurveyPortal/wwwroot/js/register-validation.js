document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    form.addEventListener("submit", function (e) {
        const errors = [];

        const email = form.querySelector("[name='Email']");
        const password = form.querySelector("[name='Password']");
        const confirmPassword = form.querySelector("[name='PasswordConfirmation']");

        form.querySelectorAll(".text-danger").forEach(el => el.textContent = "");

        if (!email.value.trim()) {
            errors.push({ field: email, message: "Email is required." });
        } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value)) {
            errors.push({ field: email, message: "Invalid email format." });
        }

        const passwordValue = password.value;
        if (!passwordValue) {
            errors.push({ field: password, message: "Password is required." });
        } else if (
            passwordValue.length < 8 ||
            !/[A-Z]/.test(passwordValue) ||
            !/[a-z]/.test(passwordValue) ||
            !/[0-9]/.test(passwordValue)
        ) {
            errors.push({
                field: password,
                message: "Password must be at least 8 characters, contain uppercase and lowercase letters, and a number."
            });
        }

        if (confirmPassword.value !== passwordValue) {
            errors.push({
                field: confirmPassword,
                message: "The password and confirmation password do not match."
            });
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