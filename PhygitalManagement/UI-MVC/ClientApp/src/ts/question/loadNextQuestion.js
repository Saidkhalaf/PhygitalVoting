const btnNextQuestion = document.querySelector("#next-question");
const noAnswerAlert = document.querySelector("#no-answer-alert");
const btnPreviousQuestion = document.querySelector("#previous-question");
let answer;

document.addEventListener("DOMContentLoaded", (event)=> {


    btnNextQuestion.addEventListener("click", function(event) {
        const questionType = document.querySelector("#question-type").getAttribute("data-question-type");
        const selectedOptions = document.querySelectorAll('input[type="radio"]:checked, input[type="checkbox"]:checked');

        if (questionType === "3") {
            const textarea = document.querySelector('textarea.form-control');
            answer = textarea ? textarea.value.trim() : "";
            if (!answer) {
                noAnswerAlert.style.display = "block";
                event.preventDefault();
                return;
            }
        } else {
            answer = Array.from(selectedOptions).map(option => option.value).join(',');
            if (selectedOptions.length === 0) {
                noAnswerAlert.style.display = "block";
                event.preventDefault();
                return;
            }
        }

            noAnswerAlert.style.display = "none";

            submitAnswerAndLoadNextQuestion(answer);
    });

    btnPreviousQuestion.addEventListener("click", function(event) {
        const currentQuestionPosition = parseInt(document.querySelector("#current-position").innerHTML);
        const subthemeId = parseInt(document.querySelector("#subtheme-id").innerHTML);

        if (currentQuestionPosition > 1) {
            loadPreviousQuestion(subthemeId, currentQuestionPosition);
        } else {
            console.log("Already at the first question.");
        }
    });
});

function generateUserAnswerId() {
    return Date.now().toString();
}

async function submitAnswerAndLoadNextQuestion(answer) {
    const currentQuestionPosition = document.querySelector("#current-position").innerHTML;
    const subthemeId = document.querySelector("#subtheme-id").innerHTML;

    const id = generateUserAnswerId();
    const data = {
        questionId: currentQuestionPosition,
        answer: answer,
        Id: id

    };

    sessionStorage.setItem(`question-${currentQuestionPosition}-answer`, answer);

    submitAnswer(data) 
        .then(() => loadNextQuestion(subthemeId, currentQuestionPosition))
        .then(data => {
            if (data && data.message === "No more questions in this subtheme") {
                const nextSubthemeId = parseInt(subthemeId) + 1;
                const totalSubthemes = document.querySelector(".navigation-buttons-Subthemes").getAttribute("data-totalsubthemes");
                if (nextSubthemeId <= totalSubthemes) {
                    // Redirect to the SubthemeCompleted view
                    window.location.href = `/Question/SubthemeCompleted?nextSubthemeId=` + nextSubthemeId;
                    const completedCurrentSubtheme = parseInt(currentQuestionPosition) >= parseInt(data.totalQuestionsWithSubtheme);
                }else  {
                    console.log("No more subthemes available.");
                }
            } else if (data) {
                replaceQuestion(data);
            }
        })
        .catch(error => {
            console.error('Error submitting answer and loading next question:', error);
        });
}

function loadNextQuestion(subthemeId, currentQuestionPosition) {
    var url = new URL(window.location.href);
    subthemeId = url.searchParams.get("subthemeId");
    sessionStorage.setItem("subthemeId", subthemeId);

    console.log('stored subthemeId:',subthemeId);


    return fetch(`/api/questions/${subthemeId}/${currentQuestionPosition}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    })
        .then(response => {
            if (!response.ok) {
                if (response.status === 404) {
                    return response.json();
                }
                throw new Error('Failed to load next question');
            }
            return response.json();
        })
        .then(data => {
            console.log(data);
            if (data && data.message === "No more questions in this subtheme") {
                console.log("Redirecting to SubthemeCompleted view");
                window.location.href = `/Question/SubthemeCompleted?nextSubthemeId=` + (subthemeId + 1);
            } else {
                replaceQuestion(data);
                return data;
            }   
        })
        .catch(error => {
            console.error('Error loading next question:', error);
        });
}

function loadPreviousQuestion() {
    console.log('loadPreviousQuestion called');
    const currentQuestionPosition = parseInt(document.querySelector("#current-position").innerHTML);
    const previousQuestionPosition = currentQuestionPosition - 1;
    const subthemeId = parseInt(document.querySelector("#subtheme-id").innerHTML);

    const storedAnswer = sessionStorage.getItem(`question-${previousQuestionPosition}-answer`);
    console.log('Retrieved answer:', storedAnswer);

    if (storedAnswer) {
        const selectedOptions = storedAnswer.split(',');
        preselectOptions(selectedOptions);
    }

    fetch(`/api/questions/${subthemeId}/previous/${currentQuestionPosition}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    })
        .then(response => response.json())
        .then(data => replaceQuestion(data))
        .catch(reason => alert("Call failed: " + reason));
}

function replaceQuestion(question) {

    const pQuestionText = document.querySelector("#question-text")
    pQuestionText.innerHTML = question.questionText;

    const pQuestionPosition = document.querySelector("#current-position")
    pQuestionPosition.innerHTML = question.position;

    const divOptions = document.querySelector(".divOptions")
    divOptions.innerHTML = ""

    for (const option of question.responseOptionsDtos){

        if(question.questionType === 0 || question.questionType === 2){
            divOptions.innerHTML += `<div>
                                 <input type="radio" id="${option.id}" name="option" value="${option.optionText}"/>
                                 <label class="btn btn-primary option-button"  for="${option.id}">${option.optionText}</label>
                                 </div>`;
        }
        else if(question.questionType === 1){
            divOptions.innerHTML += `<div>
                                 <input type="checkbox" id="${option.id}" name="option" value="${option.optionText}"/>
                                 <label class="btn btn-primary option-button" for="${option.id}">${option.optionText}</label>
                                 </div>`;
        }
    }

    if(question.questionType === 3){
        divOptions.innerHTML += `<div>
                                 <textarea class="form-control" rows="10" cols="70" name="option"></textarea>
                                 </div>`;
    }

    const textArea = document.querySelector('textarea[name="option"]');
    if (textArea) {
        textArea.addEventListener('input', function (event) {
            answer = event.target.value.trim();
        });
        answer = textArea.value.trim();
    }
    const storedAnswer = sessionStorage.getItem(`question-${question.position}-answer`);
    if (storedAnswer) {
        const selectedOptions = storedAnswer.split(',');
        preselectOptions(selectedOptions);
    }
}

function preselectOptions(selectedOptions) {
    console.log('preselectOptions called with:', selectedOptions);
    selectedOptions.forEach(option => {
        const button = Array.from(document.querySelectorAll('label')).find(label => {
            const associatedInput = document.querySelector(`input[value="${label.textContent.trim()}"]`);
            return associatedInput && associatedInput.value.trim() === option;
        });
        if (button) {
            button.classList.add('btn-primary');
            const input = document.querySelector(`input[value="${option}"]`);
            if (input) {
                input.checked = true;
            }
            console.log('Option preselected:', option);
        } else {
            console.log('Option not found:', option);
        }
    });
}


function submitAnswer(data) {
    return fetch(`/api/questions/answers`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to submit answer');
            }
        })
        .catch(error => {
            console.error('Error submitting answer:', error);
        });
}



