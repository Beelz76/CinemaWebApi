async function getScreeningPrices() {
    try {
        const response = await fetch('https://localhost:7172/api/ScreeningPrice/GetScreeningPrices', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const screeningPrices = await response.json();

            const screeningPriceTable = document.getElementById('screeningPriceTable');
            screeningPriceTable.innerHTML = '';

            for (const screeningPrice of screeningPrices) {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${screeningPrice.screeningPriceUid}</td>
                <td>${screeningPrice.price}</td>
                <td style="text-align: center;"><input type="checkbox" value="${screeningPrice.screeningPriceUid}"></td>`;
                screeningPriceTable.appendChild(row);
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

async function createScreeningPrice() {
    const screeningPrice = document.getElementById('screeningPrice').value;

    if (!screeningPrice) {
        alert('Введите цену');
        return;
    }

    try {
        const response = await fetch(`https://localhost:7172/api/ScreeningPrice/CreateScreeningPrice?price=${screeningPrice}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            await getScreeningPrices();
        } else {
            console.log(data);
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function updateScreeningPrice() {
    const screeningPrice = document.getElementById('screeningPrice').value;
    const selectedCheckboxes = document.querySelectorAll('#screeningPriceTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length !== 1) {
        alert('Выберите одну цену');
        return;
    }

    if (!screeningPrice) {
        alert('Введите цену');
        return;
    }

    const uid = selectedCheckboxes[0].value;

    try {
        const response = await fetch(`https://localhost:7172/api/ScreeningPrice/UpdateScreeningPrice?screeningPriceUid=${uid}&price=${screeningPrice}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            await getScreeningPrices();
        } else {
            console.log(data);
            throw new Error('Не удалось изменить цену');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function deleteScreeningPrice() {
    const selectedCheckboxes = document.querySelectorAll('#screeningPriceTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы одну цену');
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
            const response = await fetch(`https://localhost:7172/api/ScreeningPrice/DeleteScreeningPrice?screeningPriceUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                console.log(data)
                await getScreeningPrices();
                 
            } else {
                console.log(data)
                throw new Error('Что-то пошло не так');
            }
        } catch (error) {
            console.error(error);
            alert('Ошибка');
        }
    }
}