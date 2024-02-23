export const BASE_URL =
  process.env.QUIZMASTER_GATEWAY ?? process.env.NEXT_PUBLIC_QUIZMASTER_GATEWAY;

export const submitAnswer = ({ id, answer, connectionId }) => {
  console.log("Submit Answe");
  console.log(answer);
  fetch(`${BASE_URL}/gateway/api/room/submitAnswer`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      QuestionId: id,
      Answer: answer,
      connectionId,
    }),
  });
};

export const downloadImage = async ({ id, setImageUrl, setHasImage }) => {
  try {
    const authToken = localStorage.getItem("token");

    const response = await fetch(
      `${BASE_URL}/gateway/api/media/download_media/${id}`,
      {
        headers: {
          Authorization: `Bearer ${authToken}`,
        },
      }
    );

    // Check if the request was successful (status code 200)
    if (response.ok) {
      const blob = await response.blob();
      const imageUrl = URL.createObjectURL(blob);

      // Set the image URL in the state or use it directly in the component
      setImageUrl(imageUrl);
    } else {
      // Handle errors
      console.error("Error downloading image:", response.statusText);
      setHasImage(false);
    }
  } catch (error) {
    console.error("Error downloading image:", error);
    setHasImage(false);
  }
};

export const partialLogin = ({
  email,
  userName,
  connection,
  push,
  notifications,
}) => {
  fetch(`${BASE_URL}/gateway/api/account/create_partial`, {
    method: "POST",
    credentials: "include",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, userName }),
  }).then((r) => {
    if (r.status === 200) {
      try {
        fetch(`${BASE_URL}/gateway/api/auth/partialLogin`, {
          method: "POST",
          credentials: "include",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ email, userName }),
        }).then(async (r) => {
          if (r.status === 200) {
            const data = await r.json();
            await connection.invoke("Login", data.token);
            localStorage.setItem("username", userName.toLowerCase());
            localStorage.setItem("token", data.token);
            push("/auth/code");
          }
        });
      } catch (error) {
        notifications.show({
          title: error,
        });
      }
    } else {
      notifications.show({
        title: "Email or username already used ",
      });
    }
  });
};
