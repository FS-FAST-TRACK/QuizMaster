import { ADMIN_URL, BASE_URL } from "@/app/util/api";
import { notifications } from "@mantine/notifications";
import CryptoJS from "crypto-js";

export const submitPin = (connection, code, params, push) => {
  try {
    connection.invoke("JoinRoom", Number.parseInt(code));
    connection.on("JoinFailed", (isFailed) => {
      if (isFailed) {
        notifications.show({ title: "Room does not exist" });
      } else {
        params.set("roomPin", Number.parseInt(code));
        push(`/room?${params.toString()}`);
        connection.invoke("GetConnectionId");
      }
    });
  } catch (ex) {
    console.log(ex);
  }
};

export const startQuiz = (connection, params) => {
  connection.invoke("StartRoom", params.get("roomPin"));
};

export const timeFormater = (seconds) => {
  const minutes = Math.floor(seconds / 60);
  seconds = seconds % 60;
  return `${String(minutes).padStart(2, "0")}:${String(seconds).padStart(
    2,
    "0"
  )}`;
};

export const goBackToLoby = (
  params,
  connection,
  push,
  setResetLeader,
  setStart,
  isAdmin
) => {
  try {
    const username = localStorage.getItem("username");
    const token = localStorage.getItem("token");

    function encodeUTF8(input) {
      return encodeURIComponent(input);
    }

    const utf8EncodedToken = encodeUTF8(token);

    const encryptedToken = CryptoJS.AES.encrypt(
      utf8EncodedToken,
      "secret_key"
    ).toString();

    localStorage.clear(); //CLEAR
    const code = params.get("roomPin");
    connection.invoke("GetRoomParticipants", code);
    setResetLeader();
    setStart(false);
    
    if(isAdmin){
      push(`${ADMIN_URL}/dashboard`)
    }else{
      push(`/?name=${username}&token=${encodeURIComponent(encryptedToken)}`);
    }
  } catch (ex) {
    console.log(ex);
  }
};
