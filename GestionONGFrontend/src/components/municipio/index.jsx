import { ContentProvider } from "./context";
import List from "./list";
import Formulario from "./formulario";
export default function Index() {
  return (
    <ContentProvider>
      <Formulario/>
      <List />
    </ContentProvider>
  );
}
