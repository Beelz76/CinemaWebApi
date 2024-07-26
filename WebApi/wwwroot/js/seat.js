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
            const seats = await response.json();

            const seatTable = document.getElementById('seatTable');
            seatTable.innerHTML = '';

            for (const seat of seats) {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${seat.seatUid}</td>
                <td>${seat.hallName}</td>
                <td>${seat.row}</td>
                <td>${seat.number}</td>
                <td style="text-align: center;"><input type="checkbox" value="${seat.seatUid}"></td>`;
                seatTable.appendChild(row);
            }
        } else {
            console.log(await response.text())
            location.reload();
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
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
            const seats = await response.json();

            const seatTable = document.getElementById('seatTable');
            seatTable.innerHTML = '';

            for (const seat of seats) {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${seat.seatUid}</td>
                <td>${hallName}</td>
                <td>${seat.row}</td>
                <td>${seat.number}</td>
                <td style="text-align: center;"><input type="checkbox" value="${seat.seatUid}"></td>`;
                seatTable.appendChild(row);
                document.getElementById('hallName').value = '';
            }
        } else {
            console.log(await response.text())
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
}

async function getScreeningSeats() {
    const uid = localStorage.getItem('selectedScreening');

    if (!uid) {
        alert('Выберите сеанс');
        window.location.href = 'userScreening.html';
        return;
    }

    try {
        const response = await fetch(`https://localhost:7172/api/Seat/GetScreeningSeats?screeningUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const seats = await response.json();

            const seatTable = document.getElementById('seatTable');
            seatTable.innerHTML = '';

            for (const seat of seats) {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${seat.row}</td>
                <td>${seat.number}</td>
                <td>${seat.status}</td>
                <td style="text-align: center;"><input type="checkbox" value="${seat.seatUid}"></td>`;
                seatTable.appendChild(row);
            }
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
}

async function createSeat() {
    const hallName = document.getElementById('hallName').value;
    const seatRow = document.getElementById('seatRow').value;
    const seatNumber = document.getElementById('seatNumber').value;

    if (!hallName || !seatRow || !seatNumber) {
        alert('Заполните необходимые поля');
        return;
    }

    try {
        const response = await fetch(`https://localhost:7172/api/Seat/CreateSeat?hallName=${hallName}&row=${seatRow}&number=${seatNumber}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            await getAllSeats();
        } else {
            console.log(data);
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function updateSeat() {
    const seatRow = document.getElementById('seatRow').value;
    const seatNumber = document.getElementById('seatNumber').value;
    const selectedCheckboxes = document.querySelectorAll('#seatTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length !== 1) {
        alert('Выберите одно место');
        return;
    }

    if (!seatRow || !seatNumber) {
        alert('Заполните необходимые поля');
        return;
    }

    const uid = selectedCheckboxes[0].value;

    try {
        const response = await fetch(`https://localhost:7172/api/seat/UpdateSeat?seatUid=${uid}&row=${seatRow}&number=${seatNumber}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            await getAllSeats();
        } else {
            console.log(data);
            throw new Error('Не удалось изменить место');
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

    for (const uid of uids) {
        try {
            const response = await fetch(`https://localhost:7172/api/Seat/DeleteSeat?seatUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                await getAllSeats();
            } else {
                console.log(data);
                throw new Error('Что-то пошло не так');
            }
        } catch (error) {
            console.error(error);
            alert('Ошибка');
        }
    }
}