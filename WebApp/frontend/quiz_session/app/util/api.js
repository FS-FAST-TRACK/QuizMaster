export const submitAnswer = ({ id, answer, connectionId }) => {
  console.log("Submit Answe");
  console.log(answer);
  fetch("https://localhost:7081/gateway/api/room/submitAnswer", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      QuestionId: id,
      Answer: answer,
      connectionId,
    }),
  });
};
