$(document).ready(function () {
    $.validator.addMethod("strongPassword", function (value, element) {
        return this.optional(element) ||
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/.test(value);
    }, "Password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter, and one number");

    $.validator.addMethod("phoneValidation", function (value, element) {
        return this.optional(element) ||
            /^(\+\d{1,3}[- ]?)?\d{10,15}$/.test(value);
    }, "Please enter a valid phone number");

    $("#profileForm").validate({
        rules: {
            FirstName: {
                required: true,
                minlength: 2,
                maxlength: 50
            },
            LastName: {
                required: true,
                minlength: 2,
                maxlength: 50
            },
            Email: {
                required: true,
                email: true
            },
            PhoneNumber: {
                phoneValidation: true
            },
            Password: {
                minlength: 8,
                strongPassword: true
            },
            ConfirmPassword: {
                equalTo: "#Password"
            }
        },
        messages: {
            FirstName: {
                required: "Please enter your first name",
                minlength: "First name must be at least 2 characters",
                maxlength: "First name cannot exceed 50 characters"
            },
            LastName: {
                required: "Please enter your last name",
                minlength: "Last name must be at least 2 characters",
                maxlength: "Last name cannot exceed 50 characters"
            },
            Email: {
                required: "Please enter your email address",
                email: "Please enter a valid email address"
            },
            PhoneNumber: {
                phoneValidation: "Please enter a valid phone number"
            },
            Password: {
                minlength: "Password must be at least 8 characters"
            },
            ConfirmPassword: {
                equalTo: "Passwords do not match"
            }
        },
        errorElement: "div",
        errorClass: "invalid-feedback",
        highlight: function (element) {
            $(element).addClass("is-invalid").removeClass("is-valid");
        },
        unhighlight: function (element) {
            $(element).addClass("is-valid").removeClass("is-invalid");
        },
        errorPlacement: function (error, element) {
            error.insertAfter(element);
        }
    });

    $("#Password").on("keyup", function () {
        const password = $(this).val();
        const strengthMeter = $("#passwordStrength");
        let strength = 0;

        if (password.length >= 8) strength++;
        if (/[A-Z]/.test(password)) strength++;
        if (/[a-z]/.test(password)) strength++;
        if (/\d/.test(password)) strength++;
        if (/[^A-Za-z0-9]/.test(password)) strength++;

        strengthMeter.removeClass("bg-danger bg-warning bg-info bg-success");

        strengthMeter.css("width", (strength * 20) + "%");

        switch (strength) {
            case 0:
            case 1:
                strengthMeter.addClass("bg-danger").text("Very Weak");
                break;
            case 2:
                strengthMeter.addClass("bg-warning").text("Weak");
                break;
            case 3:
                strengthMeter.addClass("bg-info").text("Medium");
                break;
            case 4:
                strengthMeter.addClass("bg-success").text("Strong");
                break;
            case 5:
                strengthMeter.addClass("bg-success").text("Very Strong");
                break;
        }
    });

    $("#profileForm").on("submit", function (e) {
        if (!$(this).valid()) {
            e.preventDefault();
            $(this).find(":input.is-invalid").first().focus();
        }
    });
});