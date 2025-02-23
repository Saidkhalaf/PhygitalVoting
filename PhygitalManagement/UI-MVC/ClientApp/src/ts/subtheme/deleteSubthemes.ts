const deleteSubthemeButtons: NodeListOf<HTMLAnchorElement> = document.querySelectorAll(".deleteSubtheme");

deleteSubthemeButtons.forEach(button => button.addEventListener("click", () => deleteSubthemes(button)));

function deleteSubthemes(button: HTMLAnchorElement): void {
    const card: HTMLElement | null = button.closest(".card");
    if (card) {
        const subthemeIdElement: HTMLElement | null = card.querySelector(".subthemeId");
        if (subthemeIdElement) {
            const subthemeId: string = subthemeIdElement.innerHTML.trim();

            if (confirm("Do you want to delete this subtheme?")) {
                fetch(`/api/Subthemes/${subthemeId}`, {
                    method: 'DELETE'
                })
                    .then(response => {
                        if (response.ok) {
                            card.remove();
                            window.location.reload();
                        } else {
                            throw new Error(`Error status ${response.status}.`);
                        }
                    })
                    .catch(error => {
                        console.error('Error deleting subtheme:', error);
                    });
            }
        }
    }
}
