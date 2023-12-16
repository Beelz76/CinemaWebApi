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
            const data = await response.json();

            const screeningPriceTable = document.getElementById('screeningPriceTable');
            screeningPriceTable.innerHTML = '';

            data.forEach(screeningPrice => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${screeningPrice.screeningPriceUid}</td>
                <td>${screeningPrice.price}</td>
                <td style="text-align: center;"><input type="checkbox" value="${screeningPrice.screeningPriceUid}"></td>`;
                screeningPriceTable.appendChild(row);
            });
        } else {
            throw new Error('Что-то пошло не так');
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

    uids.forEach(async (uid) => {
        try {
            const response = await fetch(`https://localhost:7172/api/ScreeningPrice/DeleteScreeningPrice?screeningPriceUid=${uid}`, {
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
                    getScreeningPrices();
                } else {
                    alert('Не получилось удалить цену');
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