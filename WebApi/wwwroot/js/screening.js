async function getAllScreenings() {
    try {
        const response = await fetch('https://localhost:7172/api/Screening/GetAllScreenings', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const screeningTable = document.getElementById('screeningTable');
            screeningTable.innerHTML = '';

            data.forEach(screening => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${screening.screeningUid}</td>
                <td>${screening.movieTtitle}</td>
                <td>${screening.movieDuration}</td>
                <td>${screening.screeningStart}</td>
                <td>${screening.screeningEnd}</td>
                <td>${screening.hallName}</td>
                <td>${screening.price}</td>
                <td style="text-align: center;"><input type="checkbox" value="${screening.screeningUid}"></td>`;
                screeningTable.appendChild(row);
            });
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function getHallScreenings() {
    const hallName = document.getElementById('hallName').value;

    try {
        const response = await fetch(`https://localhost:7172/api/Screening/GetHallScreenings`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
            body: JSON.stringify(hallName)
        });

        if (response.ok) {
            const data = await response.json();

            const screeningTable = document.getElementById('screeningTable');
            screeningTable.innerHTML = '';

            data.forEach(screening => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${screening.screeningUid}</td>
                <td>${screening.movieTtitle}</td>
                <td>${screening.movieDuration}</td>
                <td>${screening.screeningStart}</td>
                <td>${screening.screeningEnd}</td>
                <td>${screening.hallName}</td>
                <td>${screening.price}</td>
                <td style="text-align: center;"><input type="checkbox" value="${screening.screeningUid}"></td>`;
                screeningTable.appendChild(row);
            });
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function getMovieScreenings() {
    const uid;

    try {
        const response = await fetch(`https://localhost:7172/api/Screening/GetMovieScreenings?movieUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const screeningTable = document.getElementById('screeningTable');
            screeningTable.innerHTML = '';

            data.forEach(screening => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${screening.screeningStart}</td>
                <td>${screening.screeningEnd}</td>
                <td>${screening.hallName}</td>
                <td>${screening.price}</td>
                <td style="text-align: center;"><input type="checkbox" value="${screening.screeningUid}"></td>`;
                screeningTable.appendChild(row);
            });
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function deleteScreening() {
    const selectedCheckboxes = document.querySelectorAll('#screeningTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы один сеанс');
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
            const response = await fetch(`https://localhost:7172/api/Screening/DeleteScreening?screeningUid=${uid}`, {
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
                    getAllScreenings();
                } else {
                    alert('Не получилось удалить сеанс');
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