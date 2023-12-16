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
            throw new Error('Что-то пошло не так');
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

            if (response.ok) {
                const data = await response.text();

                if (data) {
                    //alert(data);
                    getGenres();
                } else {
                    alert('Не получилось удалить жанр');
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