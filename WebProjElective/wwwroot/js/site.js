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

window.onclick = (event) => {
    if (!event.target.closest('.search-form') && !event.target.closest('#search-btn')) {
        searchForm.classList.remove('active');
    }
    if (!event.target.closest('.login-form') && !event.target.closest('#login-btn')) {
        loginForm.classList.remove('active');
    }
    if (!event.target.closest('.navbar') && !event.target.closest('#menu-btn')) {
        navbar.classList.remove('active');
    }
}
document.addEventListener('DOMContentLoaded', function () {
    const registerLink = document.getElementById('register-link');
    const registrationForm = document.getElementById('registration-form');
    const closeRegistration = document.getElementById('close-registration');
    const loginForm = document.getElementById('login-form');
    const blurBackground = document.getElementById('blur-background');

    registerLink.addEventListener('click', function (event) {
        event.preventDefault();
        registrationForm.classList.add('active');
        blurBackground.style.display = 'block';
        loginForm.classList.remove('active'); // Hide login form
    });

    closeRegistration.addEventListener('click', function () {
        registrationForm.classList.remove('active');
        blurBackground.style.display = 'none';
    });

    // Close the form if the user clicks outside of the form
    blurBackground.addEventListener('click', function () {
        registrationForm.classList.remove('active');
        blurBackground.style.display = 'none';
    });

    // Also close the form when pressing the Escape key
    window.addEventListener('keydown', function (event) {
        if (event.key === 'Escape') {
            registrationForm.classList.remove('active');
            blurBackground.style.display = 'none';
        }
    });
});

document.getElementById('user-btn').addEventListener('click', function (event) {
    event.stopPropagation(); // Prevent the event from bubbling up to the window
    document.getElementById('user-info').classList.toggle('show');
});

window.onclick = function (event) {
    if (!event.target.matches('.user-info-button') && !event.target.closest('.user-info-container')) {
        var dropdowns = document.getElementsByClassName("user-info-dropdown");
        for (var i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}


//const submitBtn = document.getElementById("submitBtn");

//submitBtn.addEventListener('click', funtion{

//})
