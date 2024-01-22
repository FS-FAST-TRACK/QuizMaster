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

export const downloadImage = async ({ url, setImageUrl }) => {
  try {
    const authToken = localStorage.getItem("token");

    const response = await fetch(`${url}`, {
      headers: {
        Authorization: `Bearer ${authToken}`,
      },
    });

    // Check if the request was successful (status code 200)
    if (response.ok) {
      const blob = await response.blob();
      const imageUrl = URL.createObjectURL(blob);

      // Set the image URL in the state or use it directly in the component
      setImageUrl(imageUrl);
    } else {
      // Handle errors
      console.error("Error downloading image:", response.statusText);
    }
  } catch (error) {
    console.error("Error downloading image:", error);
  }
};
