export const submitAnswer = ({ id, answer, connectionId }) => {
  fetch("https://localhost:7081/gateway/api/set/submitAnswer", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      QuestionId: id,
      Answer: answer,
      connectionId,
    }),
  })
    .then((r) => r.json())
    .then((d) => {
      console.log(d.message);
    });
};
