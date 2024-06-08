let searchForm = document.querySelector('.search-form');

document.querySelector('#search-btn').onclick = () => {
    searchForm.classList.toggle('active');
}

let loginForm = document.querySelector('.login-form');

document.querySelector('#login-btn').onclick = () => {
    loginForm.classList.toggle('active');
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

document.querySelectorAll('.close-btn').forEach(button => {
    button.addEventListener('click', function () {
        this.parentElement.style.display = 'none';
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
