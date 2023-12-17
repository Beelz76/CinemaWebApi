async function getHalls() {
    try {
        const response = await fetch('https://localhost:7172/api/Hall/GetHalls', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const hallTable = document.getElementById('hallTable');
            hallTable.innerHTML = '';

            data.forEach(hall => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${hall.hallUid}</td>
                <td>${hall.name}</td>
                <td>${hall.capacity}</td>
                <td style="text-align: center;"><input type="checkbox" value="${hall.hallUid}"></td>`;
                hallTable.appendChild(row);
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

async function createHall() {
    const hallName = document.getElementById('hallName').value;

    if (!hallName) {
        alert('Введите название зала');
        return;
    }

    try {
        const response = await fetch(`https://localhost:7172/api/Hall/CreateHall?name=${hallName}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);;
            getHalls();
        } else {
            console.log(data);
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function updateHall() {
    const hallName = document.getElementById('hallName').value;
    const selectedCheckboxes = document.querySelectorAll('#hallTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length !== 1) {
        alert('Выберите один зал');
        return;
    }

    if (!hallName) {
        alert('Введите название зала');
        return;
    }

    const uid = selectedCheckboxes[0].value;

    try {
        const response = await fetch(`https://localhost:7172/api/Hall/UpdateHall?hallUid=${uid}&name=${hallName}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            getHalls();
        } else {
            console.log(data);
            throw new Error('Не удалось изменить зал');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function deleteHall() {
    const selectedCheckboxes = document.querySelectorAll('#hallTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы один зал');
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
            const response = await fetch(`https://localhost:7172/api/Hall/DeleteHall?hallUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                getHalls();
                 
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