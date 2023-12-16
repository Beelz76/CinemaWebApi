async function getDirectors() {
    try {
        const response = await fetch('https://localhost:7172/api/Director/GetDirectors', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const directorTable = document.getElementById('directorTable');
            directorTable.innerHTML = '';

            data.forEach(director => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${director.directorUid}</td>
                <td>${director.fullName}</td>
                <td style="text-align: center;"><input type="checkbox" value="${director.directorUid}"></td>`;
                directorTable.appendChild(row);
            });
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function deleteDirector() {
    const selectedCheckboxes = document.querySelectorAll('#directorTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы одного режиссера');
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
            const response = await fetch(`https://localhost:7172/api/Director/DeleteDirector?directorUid=${uid}`, {
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
                    getDirectors();
                } else {
                    alert('Не получилось удалить режиссера');
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