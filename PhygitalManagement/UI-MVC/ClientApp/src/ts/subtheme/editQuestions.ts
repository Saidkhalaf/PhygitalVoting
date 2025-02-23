import "./addResponseOptions.ts";

function setupListeners() {
    const updateButton = document.querySelector(".updateQuestion") as HTMLButtonElement | null;
    if (updateButton) {
        updateButton.addEventListener("click", () => updateQuestion());
    }
    const radios = document.getElementsByName('.questiontypeOptions input');
    for (const radio of radios) {
        radio.addEventListener('change', function() {
            const el = radio as HTMLInputElement;
            showDivBasedOnSelection(el.value);
        });
    }
    const removeButtons = document.querySelectorAll(".btn-outline-danger");
    removeButtons.forEach(removeButton => {
        removeButton.addEventListener("click", () => removeQuestion(removeButton as HTMLElement));
    });
}
function showDivBasedOnSelection(questiontype:string){
    const divs = document.querySelectorAll('.responseOptionsContainer');
    divs.forEach(div => {
        const container = div as HTMLElement;
        container.style.display = 'none'
    }); 
    const responseOptionsContainerId:string = getResponseOptionsContainerId(+questiontype);
    const selectedDiv = document.getElementById(responseOptionsContainerId);
    if (selectedDiv) {
        selectedDiv.style.display = 'block'; 
    }
}

function getQuestionIDByElement(): string {
    const path = window.location.pathname;
    const parts = path.split("/");
    return parts[parts.length - 1];
}

function updateQuestion() {
    const questionId = getQuestionIDByElement();
    const questionDescription = getQuestionDescription();
    const subthemeId = getSubthemeId();
    const questionType = getQuestionType();

    if (questionType === null) {
        console.error("Question type not selected.");
        return;
    }

    const responseOptionsContainerId = getResponseOptionsContainerId(questionType);
    const responseOptions = getResponseOptions(responseOptionsContainerId);

    const updatedQuestion = {
        QuestionText: questionDescription,
        QuestionType: questionType,
        ResponseOptions: responseOptions
    };

    fetch(`/api/Questions/${questionId}`, {
        method: 'PUT',
        body: JSON.stringify(updatedQuestion),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (!response.ok) {
                throw Error(`Received status code ${response.status}.`);
            } else {
                window.location.href = '/Subtheme/Edit/' + subthemeId;
            }
        })
        .catch(error => {
            console.error('Error updating Question:', error);
        });
}

function getQuestionDescription(): string | null {
    const descriptionElement = document.getElementById("description") as HTMLInputElement | null;
    return descriptionElement?.value || null;
}

function getSubthemeId(): string | null {
    const subthemeIdElement = document.getElementById("subthemeId") as HTMLInputElement | null;
    return subthemeIdElement?.value || null;
}

function getQuestionType(): number | null {
    const radios = document.getElementsByName('option');
    for (const radio of radios) {
        const radioElement = radio as HTMLInputElement;
        if (radioElement.checked) {
            return +radioElement.value;
        }
    }
    return null;
}

function getResponseOptionsContainerId(questionType: number): string {
    switch (questionType) {
        case 0:
            return "SingleResponseOptionsContainer";
        case 1:
            return "MultipleResponseOptionsContainer";
        case 2:
            return "RangeResponseOptionsContainer";
        case 3:
            return "OpenResponseOptionsContainer";
    }
    return "";
}

function getResponseOptions(containerId: string | null): { OptionText: string }[] {
    if (!containerId) return [];

    const responseOptions = document.querySelectorAll(`#${containerId} .responseOptions`);
    return Array.from(responseOptions).map(o => ({ OptionText: (o as HTMLInputElement).value }));
}

function removeQuestion(removeButton: HTMLElement) {
    const parentElement = removeButton.parentElement;
    if (parentElement) {
        parentElement.remove();
    }
}

setupListeners();
