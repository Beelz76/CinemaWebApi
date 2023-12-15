async function getCountries() {
    try {
        const response = await fetch('https://localhost:7172/api/Country/GetCountries', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        const data = await response.json();
        const countryTable = document.getElementById('countryTable');
        countryTable.innerHTML = '';

        data.forEach(country => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${country.countryUid}</td>
                <td>${country.name}</td>
                <td><input type="checkbox" value="${country.countryUid}"></td>
            `;
            countryTable.appendChild(row);
        });
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}


function createCountry() {
    const countryName = document.getElementById('countryName').value;

    if (!countryName) {
        alert('Введите название страны');
        return;
    }

    const data = {
        name: countryName
    }

    fetch('https://localhost:7172/api/Country/CreateCountry', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('userToken')}`
        },
        body: JSON.stringify(data)
    })
        .then(response => response.text())
        .then(data => {
            console.log(data);
            getCountries();
        })
        .catch(error => {
            console.error(error);
            alert('Ошибка');
        });
}

function updateCountry() {
    const countryName = document.getElementById('countryName').value;
    const selectedCheckboxes = document.querySelectorAll('#countryTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length !== 1) {
        alert('Выберите только одну страну');
        return;
    }

    if (!countryName) {
        alert('Введите название страны');
        return;
    }

    const countryUid = selectedCheckboxes[0].value;

    const data = {
        name: countryName
    }

    fetch(`https://localhost:7172/api/Country/UpdateCountry?countryUid=${countryUid}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('userToken')}`
        },
        body: JSON.stringify(data)
    })
        .then(response => response.text())
        .then(data => {
            console.log(data);
            getCountries();
        })
        .catch(error => {
            console.error(error);
            alert('Ошибка');
        });
}

async function deleteCountry() {
    const selectedCheckboxes = document.querySelectorAll('#countryTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы одну страну');
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
            const response = await fetch(`https://localhost:7172/api/Country/DeleteCountry?countryUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (data) {
                //alert(data);
                getCountries();
            } else {
                alert('Не получилось удалить страну(ы)');
            }
        } catch (error) {
            console.error(error);
            alert('Ошибка');
        }
    });
}
