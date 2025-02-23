document.addEventListener("DOMContentLoaded", function () {
    const updateButton = document.querySelector(".updateSubthemes") as HTMLButtonElement | null;

    if (updateButton) {
        updateButton.addEventListener("click", updateSubthemes);
    }
});

function getIdOfSubtheme(): number {
    const path = window.location.pathname;
    const subthemeId = path.substring(path.lastIndexOf("/") + 1);
    return parseInt(subthemeId, 10);
}

function updateSubthemes(): void {
    const subthemeId = getIdOfSubtheme();
    const subthemeName = (document.getElementById("name") as HTMLInputElement).value;
    const flowId = (document.getElementById("flowId") as HTMLInputElement).value;
    const subthemeDesc = (document.getElementById("description") as HTMLInputElement).value;
    const radios = document.getElementsByName('option') as NodeListOf<HTMLInputElement>;

    const updatedSubtheme = {
        Name: subthemeName,
        Description: subthemeDesc
        
    };

    fetch(`/api/Subthemes/${subthemeId}`, {
        method: 'PUT',
        body: JSON.stringify(updatedSubtheme),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Received status code ${response.status}.`);
            } else {
                window.location.href = `http://localhost:5049/Flow?flowId=${flowId}`;
            }
        })
        .catch(error => {
            console.error('Error updating subtheme:', error);
        });
}
