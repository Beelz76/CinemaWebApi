async function GetCountries() {
    const response = await fetch('https://localhost:7172/api/Country/GetCountries');

    if (response.ok) {
        const countries = await response.json();
        const tableBody = document.getElementById('countriesTable').getElementsByTagName('tbody')[0];

        while (tableBody.firstChild) {
            tableBody.removeChild(tableBody.firstChild);
        }

        countries.forEach(country => {
            const row = tableBody.insertRow();
            const uidCell = row.insertCell();
            const nameCell = row.insertCell();

            uidCell.textContent = country.countryUid;
            nameCell.textContent = country.name;
        });
    } else {
        alert('Не удалось получить список стран');
    }
}

document.getElementById('getCountries').addEventListener('click', GetCountries);