let searchForm = document.querySelector('.search-form');

document.querySelector('#search-btn').onclick = () => {
    searchForm.classList.toggle('active');
}

let loginForm = document.querySelector('.login-form');

document.querySelector('#login-btn').onclick = () => {
    loginForm.classList.toggle('active');
}

let navbar = document.querySelector('.navbar');

document.querySelector('#menu-btn').onclick = () => {
    navbar.classList.toggle('active');
}

document.addEventListener('DOMContentLoaded', function () {
    const registerLink = document.getElementById('register-link');
    const registrationForm = document.getElementById('registration-form');
    const closeRegistration = document.getElementById('close-registration');
    const loginForm = document.getElementById('login-form');

    registerLink.addEventListener('click', function (event) {
        event.preventDefault();
        registrationForm.classList.add('active');
        loginForm.classList.remove('active'); // Hide login form
    });

    closeRegistration.addEventListener('click', function () {
        registrationForm.classList.remove('active');
    });
});

const submitBtn = document.getElementById("submitBtn");

submitBtn.addEventListener('click', funtion{

})
