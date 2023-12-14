async function login() {
    var login = document.getElementById('login').value;
    var password = document.getElementById('password').value;

    var credentials = {
        login: login,
        password: password
    }

    var response = await fetch('https://localhost:7172/api/User/Login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(credentials)
        });

    if (response.ok) {
        var data = await response.json();

        if (data.token) {
            localStorage.setItem('userToken', data.token);

            var decodedToken = JSON.parse(atob(data.token.split('.')[1]));
            var userRole = decodedToken.role;

            if (userRole === 'Admin') {
                window.location.href = 'admin.html';
            } else if (userRole === 'User') {
                window.location.href = 'country.html';
            } else {
                alert('Во время авторизации произошла ошибка');
            }
        } else {
            alert('Авторизация не удалась');
        }
    } else {
        alert('Неправильный логин или пароль');
    }   
}

async function register() {
    var fullName = document.getElementById('fullName').value;
    var login = document.getElementById('login').value;
    var password = document.getElementById('password').value;

    var credentials = {
        fullName: fullName,
        login: login,
        password: password
    };

    var response = await fetch('https://localhost:7172/api/User/Register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(credentials)
    });

    if (response.ok) {
        window.location.href = 'country.html';
    } else {
        alert('Регистрация не удалась');
    }
}