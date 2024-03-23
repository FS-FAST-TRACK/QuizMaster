export function isCorrectAnswer(participantAnswer, correctAnswer) {
  const hasMultipleAnswers = correctAnswer.split("|").length > 1;

  // If there is participant answer then return false
  if (!participantAnswer) return false;

  if (hasMultipleAnswers) {
    // Trim spaces and lowered casings
    const trimmedAnswers = correctAnswer
      .split("|")
      .map((answer) => answer.trim().toLowerCase());
    if (trimmedAnswers.includes(participantAnswer.trim().toLowerCase())) {
      return true;
    } else {
      return false;
    }
  } else {
    if (
      correctAnswer.trim().toLowerCase() ===
      participantAnswer.trim().toLowerCase()
    ) {
      return true;
    } else {
      return false;
    }
  }
}
