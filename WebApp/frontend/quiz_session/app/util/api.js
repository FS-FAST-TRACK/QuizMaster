import { createFileName } from "use-react-screenshot";

export const BASE_URL = process.env.NEXT_PUBLIC_QUIZMASTER_GATEWAY;
export const ADMIN_URL = process.env.QUIZMASTER_ADMIN;

export const submitAnswer = ({ id, answer, connectionId }) => {
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
      try {
        fetch(`${BASE_URL}/gateway/api/auth/partialLogin`, {
          method: "POST",
          credentials: "include",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ email, userName }),
        })
          .then(async (r) => {
            if (r.status === 200) {
              const data = await r.json();
              await connection.invoke("Login", data.token);
              localStorage.setItem("username", userName.toLowerCase());
              localStorage.setItem("token", data.token);
              push("/auth/code");
            } else if (r.status === 401) {
              notifications.show({
                title: "Account is not [on-the-go]",
              });
            }
          })
          .catch((error) => {
            notifications.show({
              title: error,
            });
          });
      } catch (error) {
        notifications.show({
          title: error,
        });
      }
    }
  });
};

export const userInfo = async (token) => {
  try {
    const response = fetch(`${BASE_URL}/gateway/api/auth/info`, {
      method: "GET",
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });
    const data = response.json();
    return data;
  } catch (error) {
    notifications.show({
      title: error,
    });
    return "";
  }
};
export const uploadScreenshot = (
  image,
  id,
  connectionId,
  { name = "img", extension = "jpg" } = {}
) => {
  // const a = document.createElement("a");
  // a.href = image;
  // a.download = createFileName(extension, name);
  // a.click();

  const token = localStorage.getItem("token");
  const blob = dataURLtoBlob(image);
  const formData = new FormData();
  formData.append("File", blob, createFileName(extension, name));
  // Now you can submit this formData to your API
  const requestOptions = {
    method: "POST",
    body: formData,
    redirect: "follow",
    credentials: "include",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };
  console.info("uploading screenshot");

  fetch(`${BASE_URL}/gateway/api/Media/upload`, requestOptions)
    .then((response) => response.json())
    .then((result) => {
      let imageId = result.fileInformation.id;
      if (imageId) {
        // Submit Screenshot Link
        const payload = {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({
            questionId: id,
            connectionId,
            screenshotLink: `${BASE_URL}/gateway/api/media/download_media/${imageId}`,
          }),
        };

        if (!token) {
          payload.credentials = "include";
        }

        fetch(`${BASE_URL}/gateway/api/room/submitScreenshot`, payload)
          .then((response) => response.json())
          .then((r) => {
            console.info(r.message);
          })
          .catch((e) => {
            console.error("Failed to submit screenshot: ", e);
          });
      }
    })
    .catch((error) => console.error(error));
};

const dataURLtoBlob = (dataURL) => {
  const parts = dataURL.split(";base64,");
  const contentType = parts[0].split(":")[1];
  const raw = window.atob(parts[1]);
  const rawLength = raw.length;
  const uInt8Array = new Uint8Array(rawLength);
  for (let i = 0; i < rawLength; ++i) {
    uInt8Array[i] = raw.charCodeAt(i);
  }
  return new Blob([uInt8Array], { type: contentType });
};
