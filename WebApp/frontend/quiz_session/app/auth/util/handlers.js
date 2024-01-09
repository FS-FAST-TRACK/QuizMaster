export const submitPin = (connection, code, params, push) => {
  try {
    connection.invoke("JoinRoom", Number.parseInt(code));
    connection.on("JoinFailed", (isFailed) => {
      if (isFailed) {
        alert("Incorrect pin");
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
  back,
  setResetLeader,
  setStart
) => {
  try {
    const code = params.get("roomPin");
    connection.invoke("GetRoomParticipants", code);
    setResetLeader();
    setStart(false);
    back();
  } catch (ex) {
    console.log(ex);
  }
};
