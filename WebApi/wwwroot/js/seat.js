async function getAllSeats() {
    try {
        const response = await fetch('https://localhost:7172/api/Seat/GetAllSeats', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const seatTable = document.getElementById('seatTable');
            seatTable.innerHTML = '';

            data.forEach(seat => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${seat.seatUid}</td>
                <td>${seat.hallName}</td>
                <td>${seat.row}</td>
                <td>${seat.number}</td>
                <td style="text-align: center;"><input type="checkbox" value="${seat.seatUid}"></td>`;
                seatTable.appendChild(row);
            });
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function getHallSeats() {
    const hallName = document.getElementById('hallName').value; 

    if (!hallName) {
        alert('Введите название зала');
        return;
    }

    try {
        const response = await fetch(`https://localhost:7172/api/Seat/GetHallSeats?hallName=${hallName}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const seatTable = document.getElementById('seatTable');
            seatTable.innerHTML = '';

            data.forEach(seat => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${seat.seatUid}</td>
                <td>${hallName}</td>
                <td>${seat.row}</td>
                <td>${seat.number}</td>
                <td style="text-align: center;"><input type="checkbox" value="${seat.seatUid}"></td>`;
                seatTable.appendChild(row);
                document.getElementById('hallName').value = '';
            });
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function deleteSeat() {
    const selectedCheckboxes = document.querySelectorAll('#seatTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы одно место');
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
            const response = await fetch(`https://localhost:7172/api/Seat/DeleteSeat?seatUid=${uid}`, {
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
                    getAllSeats();
                } else {
                    alert('Не получилось удалить место');
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