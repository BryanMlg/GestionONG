import { ContentProvider } from "./context";
import List from "./list";
export default function Index() {
  return (
    <ContentProvider>
      <List />
    </ContentProvider>
  );
}
