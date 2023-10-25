import Home from "@/app/page";
import { screen } from "@testing-library/react";
import { MantineProvider } from "@mantine/core";
import { render } from "@/test-utils/render";
it("Should have Username", () => {
	render(<Home />); // arrange

	const elem = screen.getByText("Username"); // act

	expect(elem).toBeInTheDocument(); // assert
});
