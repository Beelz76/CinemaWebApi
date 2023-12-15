async function login() {
    const login = document.getElementById('login').value;
    const password = document.getElementById('password').value;

    const credentials = {
        login: login,
        password: password
    };

    try {
        const response = await fetch('https://localhost:7172/api/User/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(credentials)
        });

        if (response.ok) {
            const data = await response.json();

            if (data.token) {
                localStorage.setItem('userToken', data.token);

                var decodedToken = JSON.parse(atob(data.token.split('.')[1]));
                const userRole = decodedToken.role;

                if (userRole === 'Admin') {
                    window.location.href = 'admin.html';
                } else if (userRole === 'User') {
                    window.location.href = 'country.html';
                } else {
                    alert('Произошла ошибка при авторизации');
                }
            } else {
                throw new Error('Неправильный логин или пароль');
            }
        } else {
            throw new Error('Неправильный логин или пароль');
        }
    } catch (error) {
        console.error(error);
        alert('Неправильный логин или пароль');
    }
}

async function register() {
    const fullName = document.getElementById('fullName').value;
    const login = document.getElementById('login').value;
    const password = document.getElementById('password').value;

    if (!fullName || !login || !password) {
        alert('Please fill in all fields');
        return;
    }

    const credentials = {
        fullName: fullName,
        login: login,
        password: password
    };

    try {
        const response = await fetch('https://localhost:7172/api/User/Register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(credentials)
        });

        if (response.ok) {
            alert('Успешная регистрация');
            window.location.href = 'country.html';
        } else {
            throw new Error('Ошибка регистрации');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка регистрации');
    }
}

async function updateUser() {
    const fullName = document.getElementById('fullName').value;
    const login = document.getElementById('login').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const confirmedPassword = document.getElementById('confirmedPassword').value;

    if (!fullName || !login || !password || !confirmedPassword) {
        alert('Заполните необходимые поля');
        return;
    }

    const userUpdate = {
        fullName: fullName,
        login: login,
        email: email,
        password: password,
        confirmedPassword: confirmedPassword
    };

    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const uid = decodedToken.nameid;

    try {
        const response = await fetch(`https://localhost:7172/api/User/UpdateUser?userUid=${uid}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
            body: JSON.stringify(userUpdate)
        });

        if (response.ok) {
            const data = await response.text();
            console.log(data);
            alert('Данные обновлены');
            location.reload(true);
        } else {
            throw new Error('Не удалось обновить данные');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function getUserInfo() {
    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const uid = decodedToken.nameid;

    try {
        const response = await fetch(`https://localhost:7172/api/User/GetUserInfo?userUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
        });

        if (response.ok) {
            const data = await response.json();
            document.getElementById('fullName').value = data.fullName;
            document.getElementById('login').value = data.login;
            document.getElementById('email').value = data.email;
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}
