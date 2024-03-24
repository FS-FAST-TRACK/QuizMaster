import useSound from "use-sound";

export function useAnswerSFX(volume = 1) {
  const [playCorrect] = useSound("/audio/correctAnswer-SFX.mp3", {
    volume: volume < 1 ? volume : 1,
  });
  const [playIncorrect] = useSound("/audio/incorrectAnswer-SFX.mp3", {
    volume: volume < 1 ? volume : 1,
  });

  return { playCorrect, playIncorrect };
}
