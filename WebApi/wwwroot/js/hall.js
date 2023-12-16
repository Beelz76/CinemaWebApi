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
            throw new Error('Что-то пошло не так');
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

            if (response.ok) {
                const data = await response.text();

                if (data) {
                    //alert(data);
                    getHalls();
                } else {
                    alert('Не получилось удалить зал');
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