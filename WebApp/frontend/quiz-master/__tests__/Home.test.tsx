import Home from "@/app/page";
import { render, screen } from "@testing-library/react";

it("Should have Docs", () => {
	render(<Home />); // arrange

	const elem = screen.getByText("Docs"); // act

	expect(elem).toBeInTheDocument(); // assert
});
