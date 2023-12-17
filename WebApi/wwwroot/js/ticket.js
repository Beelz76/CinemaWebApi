async function getAllTickets() {
    try {
        const response = await fetch('https://localhost:7172/api/Ticket/GetAllTickets', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const ticketTable = document.getElementById('ticketTable');
            ticketTable.innerHTML = '';

            data.forEach(ticket => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${ticket.ticketUid}</td>
                <td>${ticket.userFullName}</td>
                <td>${ticket.movieTitle}</td>
                <td>${ticket.movieDuration}</td>
                <td>${ticket.screeningStart}</td>
                <td>${ticket.screeningEnd}</td>
                <td>${ticket.price}</td>
                <td>${ticket.hallName}</td>
                <td>${ticket.row}</td>
                <td>${ticket.number}</td>
                <td style="text-align: center;"><input type="checkbox" value="${ticket.ticketUid}"></td>`;
                ticketTable.appendChild(row);
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

async function getUserTickets() {
    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const uid = decodedToken.nameid;

    try {
        const response = await fetch(`https://localhost:7172/api/Ticket/GetUserTickets?userUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const ticketTable = document.getElementById('ticketTable');
            ticketTable.innerHTML = '';

            data.forEach(ticket => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${ticket.movieTitle}</td>
                <td>${ticket.movieDuration}</td>
                <td>${ticket.screeningStart}</td>
                <td>${ticket.screeningEnd}</td>
                <td>${ticket.price}</td>
                <td>${ticket.hallName}</td>
                <td>${ticket.row}</td>
                <td>${ticket.number}</td>
                <td style="text-align: center;"><input type="checkbox" value="${ticket.ticketUid}"></td>`;
                ticketTable.appendChild(row);
            });
        } else {
            console.log(await response.text())
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
}

async function getScreeningTickets() {
    const uid = document.getElementById('screeningUid').value;

    try {
        const response = await fetch(`https://localhost:7172/api/Ticket/GetScreeningTickets?screeningUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const ticketTable = document.getElementById('ticketTable');
            ticketTable.innerHTML = '';

            data.forEach(ticket => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${ticket.ticketUid}</td>
                <td>${ticket.userFullName}</td>
                <td>${ticket.movieTitle}</td>
                <td>${ticket.movieDuration}</td>
                <td>${ticket.screeningStart}</td>
                <td>${ticket.screeningEnd}</td>
                <td>${ticket.price}</td>
                <td>${ticket.hallName}</td>
                <td>${ticket.row}</td>
                <td>${ticket.number}</td>
                <td style="text-align: center;"><input type="checkbox" value="${ticket.ticketUid}"></td>`;
                ticketTable.appendChild(row);
            });
        } else {
            console.log(await response.text())
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
}

async function createTicket() {
    const selectedCheckboxes = document.querySelectorAll('#seatTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы одно место');
        return;
    }

    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const userUid = decodedToken.nameid;
    const screeningUid = localStorage.getItem('selectedScreening');

    let uids = [];
    for (let i = 0; i < selectedCheckboxes.length; i++) {
        if (selectedCheckboxes[i].checked) {
            uids.push(selectedCheckboxes[i].value);
        }
    }

    uids.forEach(async (uid) => {
        try {
            const response = await fetch(`https://localhost:7172/api/Ticket/CreateTicket?userUid=${userUid}&seatUid=${uid}&screeningUid=${screeningUid}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                location.reload(true);
            } else {
                console.log(data)
                throw new Error('Что-то пошло не так');
            }
        } catch (error) {
            console.error(error);
            alert('Место занято');
            location.reload(true);
        }
    });
}

async function deleteTicket() {
    const selectedCheckboxes = document.querySelectorAll('#ticketTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы один билет');
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
            const response = await fetch(`https://localhost:7172/api/Ticket/DeleteTicket?ticketUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                if (window.location.href === 'https://localhost:7172/html/user/userTicket.html') {
                    console.log(data);
                    location.reload(true);
                } else if (window.location.href === 'https://localhost:7172/html/admin/ticket.html') {
                    console.log(data);
                    getAllTickets();
                }              
            } else {
                console.log(data)
                throw new Error('Что-то пошло не так');
            }
        } catch (error) {
            console.error(error);
      
        }
    });
}