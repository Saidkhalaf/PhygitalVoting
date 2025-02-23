const activateButtons: NodeListOf<HTMLButtonElement> = document.querySelectorAll(".activate-deactivate-Subtheme");

activateButtons.forEach(button => button.addEventListener("click", () => activateDisableSubtheme(button)));

function activateDisableSubtheme(button: HTMLButtonElement): void {
    const card: HTMLElement | null = button.closest(".card");
    if (card) {
        const viewQuestionButton: HTMLElement | null = card.querySelector(".viewQuestionButton");
        const isActivated: boolean = card.dataset.activated === "true";
        card.setAttribute("data-activated", isActivated ? "false" : "true");

        if (isActivated) {
            card.style.backgroundColor = "";
            if (viewQuestionButton) {
                viewQuestionButton.style.display = "inline-block";
            }
        } else {
            card.style.backgroundColor = "red";
            if (viewQuestionButton) {
                viewQuestionButton.style.display = "none";
            }
        }
    }
}
