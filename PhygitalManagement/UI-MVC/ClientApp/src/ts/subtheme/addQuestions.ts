import { setupAddResponseButtons } from "./addResponseOptions";
import { deleteQuestion } from "./deleteQuestions";

let selectedQuestionType: string | null = null;
let usedPositions = new Set<number>();

const addSingleQuestionButton = document.getElementById('addSingleQuestion');
const addMultipleQuestionButton = document.getElementById('addMultipleQuestion');
const addRangeQuestionButton = document.getElementById('addRangeQuestion');
const addOpenQuestionButton = document.getElementById('addOpenQuestion');

if (addSingleQuestionButton) {
    addSingleQuestionButton.addEventListener('click', () => {
        selectedQuestionType = 'single';
        addQuestions();
    });
}

if (addMultipleQuestionButton) {
    addMultipleQuestionButton.addEventListener('click', () => {
        selectedQuestionType = 'multiple';
        addQuestions();
    });
}

if (addRangeQuestionButton) {
    addRangeQuestionButton.addEventListener('click', () => {
        selectedQuestionType = 'range';
        addQuestions();
    });
}

if (addOpenQuestionButton) {
    addOpenQuestionButton.addEventListener('click', () => {
        selectedQuestionType = 'open';
        addQuestions();
    });
}

setupAddResponseButtons();

function getNextAvailablePosition(): number {
    let position = 1;
    while (usedPositions.has(position)) {
        position++;
    }
    return position;
}

function showConfirmationMessage(confirmationMessageId: string) {
    const confirmationMessage = document.getElementById(confirmationMessageId);
    if (confirmationMessage) {
        confirmationMessage.style.display = 'block';
        setTimeout(() => {
            confirmationMessage.style.display = 'none';
        }, 2000);
    }
}

function addQuestions(): void {
    if (!selectedQuestionType) {
        console.error("Unknown question type selected.");
        return;
    }

    let questionType: number;
    let positionElementId: string;
    let textElementId: string;

    switch (selectedQuestionType) {
        case "single":
            questionType = 0;
            positionElementId = "questionPositionSingle";
            textElementId = "questionSingle";
            break;
        case "multiple":
            questionType = 1;
            positionElementId = "questionPositionMultiple";
            textElementId = "questionMultiple";
            break;
        case "range":
            questionType = 2;
            positionElementId = "questionPositionRange";
            textElementId = "questionRange";
            break;
        case "open":
            questionType = 3;
            positionElementId = "questionPositionOpen";
            textElementId = "questionOpen";
            break;
        default:
            console.error("Unknown question type selected.");
            return;
    }

    const subthemeId = (document.getElementById("subthemeId") as HTMLInputElement)?.value;
    const positionElement = document.getElementById(positionElementId) as HTMLInputElement | null;
    const textElement = document.getElementById(textElementId) as HTMLTextAreaElement | null;

    if (!positionElement || !textElement) {
        console.error("Position element or text element is not found.");
        return;
    }

    const positionErrorElement = document.getElementById(`positionError${selectedQuestionType.charAt(0).toUpperCase() + selectedQuestionType.slice(1)}`) as HTMLElement;
    const questionErrorElement = document.getElementById(`questionError${selectedQuestionType.charAt(0).toUpperCase() + selectedQuestionType.slice(1)}`) as HTMLElement;
    const responseOptionErrorElement = document.getElementById(`responseOptionError${selectedQuestionType.charAt(0).toUpperCase() + selectedQuestionType.slice(1)}`) as HTMLElement;

    positionErrorElement.textContent = "";
    questionErrorElement.textContent = "";
    if (questionType !== 3) {
        responseOptionErrorElement.textContent = "";
    }

    const position = positionElement.value.trim();
    const text = textElement.value.trim();

    let hasError = false;

    if (!position) {
        positionErrorElement.textContent = "Position is required.";
        hasError = true;
    } else if (!/^\d+$/.test(position)) {
        positionErrorElement.textContent = "Position must be a number.";
        hasError = true;
    } else {
        const parsedPosition = parseInt(position);
        if (parsedPosition <= 0) {
            positionErrorElement.textContent = "Position must be a positive number greater than 0.";
            hasError = true;
        } else if (usedPositions.has(parsedPosition)) {
            positionErrorElement.textContent = "Position already used. Please choose a different position.";
            hasError = true;
        } else if (parsedPosition !== getNextAvailablePosition()) {
            positionErrorElement.textContent = `Position must be ${getNextAvailablePosition()}.`;
            hasError = true;
        }
    }

    if (!text) {
        questionErrorElement.textContent = "Question is required.";
        hasError = true;
    }

    let responseOptions: string[] = [];
    if (questionType !== 3) {
        const responseOptionsContainerId = `responseOptionsContainer${selectedQuestionType.charAt(0).toUpperCase() + selectedQuestionType.slice(1)}`;
        const responseOptionsContainer = document.getElementById(responseOptionsContainerId);

        if (responseOptionsContainer) {
            responseOptions = Array.from(responseOptionsContainer.getElementsByClassName('responseOptions') as HTMLCollectionOf<HTMLTextAreaElement>)
                .map(option => option.value.trim())
                .filter(optionText => optionText !== '');

            if (responseOptions.length === 0) {
                responseOptionErrorElement.textContent = "At least one response option is required.";
                hasError = true;
            }
        }
    }

    if (hasError) {
        return;
    }

    const postQuestion = {
        Position: parseInt(position, 10),
        QuestionText: text,
        QuestionType: questionType,
        ResponseOptionsDtos: responseOptions.map(option => ({ OptionText: option }))
    };

    fetch(`/api/Subthemes/${subthemeId}/questions`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(postQuestion),
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(errorData => {
                    throw new Error(JSON.stringify(errorData));
                });
            }

            usedPositions.add(parseInt(position, 10));

            let optionsHTML = "";
            postQuestion.ResponseOptionsDtos.forEach((option, index) => {
                optionsHTML += `<p>Option ${index + 1}: ${option.OptionText}</p>`;
            });

            const questionList = document.getElementById("createdQuestion");
            const newQuestion = document.createElement("div");
            newQuestion.className = "card question-card";

            newQuestion.innerHTML = `
            <div class="card-body" id="questionId">
                <p><strong>Position:</strong> ${position}</p>
                <p><strong>Question:</strong> ${text}</p>
                ${optionsHTML ? `<p><strong>Options:</strong></p>${optionsHTML}` : ''}
                <a class="btn btn-outline-danger mt-2 removeQuestionButton" data-position="${position}">Remove</a>
            </div>`;

            questionList?.appendChild(newQuestion);

            if (selectedQuestionType) {
                showConfirmationMessage(`confirmationMessage${selectedQuestionType.charAt(0).toUpperCase() + selectedQuestionType.slice(1)}`); // Show confirmation message for the appropriate question type
            }
            
        })
        .catch(error => {
            console.error('Error adding question:', error);
            const errorData = JSON.parse(error.message);

            if (errorData.errors) {
                if (errorData.errors.Position) {
                    positionErrorElement.textContent = errorData.errors.Position.join(', ');
                }
                if (errorData.errors.QuestionText) {
                    questionErrorElement.textContent = errorData.errors.QuestionText.join(', ');
                }
                if (errorData.errors.ResponseOptions) {
                    responseOptionErrorElement.textContent = errorData.errors.ResponseOptions.join(', ');
                }
            } else {
                alert(`Failed to add question: ${error.message}`);
            }
        });
}

document.addEventListener("click", (event) => {
    const button = event.target as HTMLElement;
    if (button && button.classList.contains("removeQuestionButton")) {
        const positionToRemove = button.getAttribute("data-position");
        const subthemeId = (document.getElementById("subthemeId") as HTMLInputElement)?.value;
        if (positionToRemove !== null && subthemeId !== null) {
            const questionElement = button.closest(".question-card") as HTMLElement;
            deleteQuestion(parseInt(subthemeId), parseInt(positionToRemove), questionElement);
            usedPositions.delete(parseInt(positionToRemove, 10));
        }
    }
});
