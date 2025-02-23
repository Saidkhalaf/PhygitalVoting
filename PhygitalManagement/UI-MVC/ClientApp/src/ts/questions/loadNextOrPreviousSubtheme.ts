const btnPreviousSubthemes : NodeListOf<HTMLButtonElement> = document.querySelectorAll<HTMLButtonElement>("#previous-subtheme");
const btnNextSubthemes : NodeListOf<HTMLButtonElement> = document.querySelectorAll<HTMLButtonElement>("#next-subtheme");
const alertInformation : HTMLElement | null = document.querySelector<HTMLElement>(".alert-info");
const totalSubthemes : number = parseInt(document.querySelector<HTMLElement>(".navigation-buttons-Subthemes")?.dataset.totalsubthemes || "0");

btnPreviousSubthemes.forEach(btnPreviousSubtheme => {
    btnPreviousSubtheme.addEventListener("click", function (event) {
        loadPreviousSubtheme(event);
    });
});

btnNextSubthemes.forEach(btnNextSubtheme => {
    btnNextSubtheme.addEventListener("click", function (event) {
        loadNextSubtheme(event);
    });
});

function loadPreviousSubtheme(event: Event): void {
    if (alertInformation) {
        const currentSubthemeId = parseInt(document.querySelector<HTMLElement>("#subtheme-id")?.innerText || "0");
        if (currentSubthemeId === 1) {
            event.preventDefault();
            alertInformation.style.display = "block";
            alertInformation.innerText = "You are currently viewing the first subtheme.";
            setTimeout(() => {
                alertInformation.style.display = "none";
            }, 3000);
        }
    }
}

function loadNextSubtheme(event: Event): void {
    if (alertInformation) {
        const currentSubthemeId = parseInt(document.querySelector<HTMLElement>("#subtheme-id")?.innerText || "0");
        if (currentSubthemeId === totalSubthemes) {
            event.preventDefault();
            alertInformation.style.display = "block";
            alertInformation.innerText = "You have reached the last subtheme.";
            setTimeout(() => {
                alertInformation.style.display = "none";
            }, 3000);
        }
    }
}

