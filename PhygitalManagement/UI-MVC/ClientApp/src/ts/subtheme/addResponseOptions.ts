export function setupAddResponseButtons(): void {
    const addResponseButtons = document.querySelectorAll(".addResponseButton");
    addResponseButtons.forEach(addResponseButton => {
        addResponseButton.addEventListener("click", handleAddResponseButtonClick);
    });
}

function handleAddResponseButtonClick(event: Event): void {
    const button: HTMLElement = event.currentTarget as HTMLElement;
    const containerId: string = button.getAttribute('data-container-id')!;
    const responseOptionsContainer: HTMLElement | null = document.getElementById(containerId);

    if (responseOptionsContainer) {
        createResponseOption(responseOptionsContainer);
    } else {
        console.error("responseOptionsContainer not found.");
    }
}

function createResponseOption(container: HTMLElement): void {
    const responseOptionWrapper = document.createElement("div");
    responseOptionWrapper.className = "response-option-wrapper mb-3 d-flex align-items-center";

    const responseOptionInput = document.createElement("textarea");
    responseOptionInput.className = "form-control responseOptions";
    responseOptionInput.placeholder = "Response Option";
    responseOptionInput.style.flex = "1";

    const removeButton = document.createElement("button");
    removeButton.className = "btn btn-outline-danger ms-2";
    removeButton.type = "button";
    removeButton.textContent = "Remove";
    removeButton.style.marginLeft = "10px";

    removeButton.addEventListener("click", () => {
        responseOptionWrapper.remove();
    });

    responseOptionWrapper.appendChild(responseOptionInput);
    responseOptionWrapper.appendChild(removeButton);

    container.appendChild(responseOptionWrapper);
}

setupAddResponseButtons();
