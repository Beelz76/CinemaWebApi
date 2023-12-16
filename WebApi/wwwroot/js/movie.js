async function getAllMovies() {
    try {
        const response = await fetch('https://localhost:7172/api/Movie/GetAllMovies', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const movieTable = document.getElementById('movieTable');
            movieTable.innerHTML = '';

            data.forEach(movie => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${movie.movieUid}</td>
                <td>${movie.title}</td>
                <td>${movie.releaseYear}</td>
                <td>${movie.duration}</td>
                <td>${movie.directors}</td>
                <td>${movie.countries}</td>
                <td>${movie.genres}</td>
                <td style="text-align: center;"><input type="checkbox" value="${movie.movieUid}"></td>`;
                movieTable.appendChild(row);
            });
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function getSingleMovie() {
    const uid = document.getElementById('movieTitle').value

    if (!uid) {
        alert('Введите uid фильма');
        return;
    }

    try {
        const response = await fetch(`https://localhost:7172/api/Movie/GetSingleMovie?movieUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const movieTable = document.getElementById('movieTable');
            movieTable.innerHTML = '';

            const row = document.createElement('tr');
            row.innerHTML = `
            <td>${data.movieUid}</td>
            <td>${data.title}</td>
            <td>${data.releaseYear}</td>
            <td>${data.duration}</td>
            <td>${data.directors}</td>
            <td>${data.countries}</td>
            <td>${data.genres}</td>
            <td style="text-align: center;"><input type="checkbox" value="${data.movieUid}"></td>`;
            movieTable.appendChild(row);
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function getMoviesInfo() {
    try {
        const response = await fetch('https://localhost:7172/api/Movie/GetMoviesInfo', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const movieTable = document.getElementById('movieTable');
            movieTable.innerHTML = '';

            data.forEach(movie => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${movie.title}</td>
                <td>${movie.releaseYear}</td>
                <td>${movie.duration}</td>
                <td>${movie.directors}</td>
                <td>${movie.countries}</td>
                <td>${movie.genres}</td>
                <td style="text-align: center;"><input type="checkbox" value="${movie.movieUid}"></td>`;
                movieTable.appendChild(row);
            });
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function deleteMovie() {
    const selectedCheckboxes = document.querySelectorAll('#movieTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы один фильм');
        return;
    }

    let uids = [];
    for (let i = 0; i < selectedCheckboxes.length; i++) {
        if (selectedCheckboxes[i].checked) {
            uids.push(selectedCheckboxes[i].value);
        }
    }

    uids.forEach(async (uid) => {
        try {
            const response = await fetch(`https://localhost:7172/api/Movie/DeleteMovie?movieUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            if (response.ok) {
                const data = await response.text();

                if (data) {
                    //alert(data);
                    getAllMovies();
                } else {
                    alert('Не получилось удалить фильм');
                }
            } else {
                throw new Error('Что-то пошло не так');
            }
        } catch (error) {
            console.error(error);
            alert('Ошибка');
        }
    });
}