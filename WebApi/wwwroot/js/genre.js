async function getGenres() {
    try {
        const response = await fetch('https://localhost:7172/api/Genre/GetGenres', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const genreTable = document.getElementById('genreTable');
            genreTable.innerHTML = '';

            data.forEach(genre => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${genre.genreUid}</td>
                <td>${genre.name}</td>
                <td style="text-align: center;"><input type="checkbox" value="${genre.genreUid}"></td>`;
                genreTable.appendChild(row);
            });
        } else {
            console.log(await response.text())
            location.reload(true);
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
}

async function createGenre() {
    const genreName = document.getElementById('genreName').value;

    if (!genreName) {
        alert('Введите название жанра');
        return;
    }

    try {
        const response = await fetch(`https://localhost:7172/api/Genre/Creategenre?name=${genreName}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);;
            getGenres();
        } else {
            console.log(data);
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function updateGenre() {
    const genreName = document.getElementById('genreName').value;
    const selectedCheckboxes = document.querySelectorAll('#genreTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length !== 1) {
        alert('Выберите один жанр');
        return;
    }

    if (!genreName) {
        alert('Введите название жанра');
        return;
    }

    const uid = selectedCheckboxes[0].value;

    try {
        const response = await fetch(`https://localhost:7172/api/Genre/UpdateGenre?genreUid=${uid}&name=${genreName}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            getGenres();
        } else {
            console.log(data);
            throw new Error('Не удалось изменить жанр');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function deleteGenre() {
    const selectedCheckboxes = document.querySelectorAll('#genreTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы один жанр');
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
            const response = await fetch(`https://localhost:7172/api/Genre/DeleteGenre?genreUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                getGenres();
                
            } else {
                console.log(data);
                throw new Error('Что-то пошло не так');
            }
        } catch (error) {
            console.error(error);
            alert('Ошибка');
        }
    });
}