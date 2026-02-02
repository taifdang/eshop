---
agent: agent
model: Claude Sonnet 4.5 (copilot)
---
Define the task to achieve, including specific requirements, constraints, and success criteria.

# Task
Refactor flow generate variant 

# Requirements
1. Product is the Aggregate Root. can be add domain method (optional)
2. Don't use dbContext directly, can be use generic repository<,> or add custom repository
3. Input example: `ProductId, Dictionary<Guid, List<Guid>> optionValues`
4. If the logic is complex, create a Service to implement the code (example: ProductService, VariantGenerator, ...)
5. Add all dependences references for this class.

# Contraints
- No delete old code
- Only comment old code and add new code using: 
/* OLD [version] */
/* NEW [version] */
- Simple and easy to maintain

# Output
- Code only
- No explanation
